using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace com.rakib.colorassistant
{
    [CustomEditor(typeof(ProjectColorSetup))]
    public class ProjectColorSetupInspector : Editor
    {
        private ProjectColorSetup _projectColorSetup;
        private RenderGrayscale _renderGrayscale;
        private RenderBSC _renderBsc;
        private List<ColorPalette> _colorPalettes = new List<ColorPalette>();
        private List<string> _colorPalettesString = new List<string>();
        private string[] _configsGUIDs;
        private int _activePaletteIndex;

        public override void OnInspectorGUI()
        {
            ColorAssistantUtils.DrawHeader();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("This is a singleton scriptable object. Use this to define your project's color palette." +
                                    " Keep only one instance of this asset", ColorAssistantUtils.GUIMessageStyle);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.Space();
            GUILayout.Label("Palette Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            //Palette Settings
            serializedObject.Update();
            var paletteSettings = serializedObject.FindProperty("paletteSettings");
            paletteSettings.objectReferenceValue = (ColorPaletteSettings) EditorGUILayout.ObjectField("Palette Settings", _projectColorSetup.paletteSettings,
                typeof(ColorPaletteSettings), false);
            serializedObject.ApplyModifiedProperties();


            EditorGUILayout.BeginHorizontal();
            //Active Palette
            if (_projectColorSetup.paletteSettings != null)
            {
                var activePalette = serializedObject.FindProperty("activePalette");
                
                //Load available color palettes
                _configsGUIDs = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(ColorPalette).Name);
                _colorPalettes = new List<ColorPalette>();
                _colorPalettesString = new List<string>();

                for (int i = 0; i < _configsGUIDs.Length; i++)
                {
                    var path = AssetDatabase.GUIDToAssetPath(_configsGUIDs[i]);
                    var colorPalette = (ColorPalette) AssetDatabase.LoadAssetAtPath(path, typeof(ColorPalette));
                    _colorPalettes.Add(colorPalette);
                    _colorPalettesString.Add(colorPalette.name);
                }
                _activePaletteIndex = _colorPalettes.IndexOf((ColorPalette) activePalette.objectReferenceValue);
                
                //show palettes popup
                _activePaletteIndex = EditorGUILayout.Popup("Palette", _activePaletteIndex, _colorPalettesString.ToArray());
                var palette = _colorPalettes[_activePaletteIndex];

                if (palette == null)
                {
                    activePalette.objectReferenceValue = null;
                }
                else if (palette.settings == _projectColorSetup.paletteSettings)
                {
                    if(activePalette.objectReferenceValue != palette)
                    {
                        activePalette.objectReferenceValue = palette;
                        serializedObject.ApplyModifiedProperties();
                        _projectColorSetup.UpdateSceneMaterialColors();
                    }
                }
                else
                {
                    activePalette.objectReferenceValue = null;
                }
                serializedObject.ApplyModifiedProperties();
            }
            else
                EditorGUILayout.HelpBox("A Palette Settings is required", MessageType.Error);


            if (GUILayout.Button("Find"))
            {
                var path = AssetDatabase.GUIDToAssetPath(_configsGUIDs[_activePaletteIndex]);
                Selection.activeObject=AssetDatabase.LoadMainAssetAtPath(path);
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();
            //Helper
//            if (GUILayout.Button("Force Update"))
//                _projectColorSetup.UpdateSceneMaterialColors();
//            
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.Space();
            GUILayout.Label("Final Color Modifier", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Use these to modify the final color." +
                            " Works in editor only. Mainly used while making gameplay videos to adjust the contrast", ColorAssistantUtils.GUIMessageStyle);
            EditorGUILayout.EndVertical();

            //Check Contrast
            var checkContrast = serializedObject.FindProperty("checkContrast");
            checkContrast.boolValue = EditorGUILayout.ToggleLeft("Check Contrast", checkContrast.boolValue);
            if (_renderGrayscale == null)
            {
                _renderGrayscale = Camera.main.transform.GetComponent<RenderGrayscale>();
                if(_renderGrayscale == null)
                    _renderGrayscale = Camera.main.gameObject.AddComponent<RenderGrayscale>();
            }
            _renderGrayscale.greyScaleAmount = checkContrast.boolValue ? 1f : 0f;
            serializedObject.ApplyModifiedProperties();
            
            //Modify Properties
            var modifyProperties = serializedObject.FindProperty("modifyProperties");
            modifyProperties.boolValue = EditorGUILayout.ToggleLeft("Modify Final Color", modifyProperties.boolValue);
            if (_renderBsc == null)
            {
                _renderBsc = Camera.main.transform.GetComponent<RenderBSC>();
                if(_renderBsc == null)
                    _renderBsc = Camera.main.gameObject.AddComponent<RenderBSC>();
            }
            if (modifyProperties.boolValue)
            {
                _renderBsc.enabled = true;
                
                var brightness = serializedObject.FindProperty("brightness");
                brightness.floatValue = EditorGUILayout.Slider("Brightness", brightness.floatValue, 0f, 2f);
                
                var saturation = serializedObject.FindProperty("saturation");
                saturation.floatValue = EditorGUILayout.Slider("Saturation", saturation.floatValue, 0f, 2f);
                
                var contrast = serializedObject.FindProperty("contrast");
                contrast.floatValue = EditorGUILayout.Slider("Contrast", contrast.floatValue, 0f, 3f);

                _renderBsc.brightness = brightness.floatValue;
                _renderBsc.saturation = saturation.floatValue;
                _renderBsc.contrast = contrast.floatValue;
            }
            else
                _renderBsc.enabled = false;
            
            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            
        }

        private void OnEnable()
        {
            _projectColorSetup = (ProjectColorSetup) target;
        }

        private void OnDisable()
        {
            
        }

        private void OnDestroy()
        {
            
        }
    }
}