using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace com.rakib.colorassistant
{
    [CustomEditor(typeof(ColorPalette))]
    public class ColorPaletteInspector : Editor
    {
        private List<string> _tempHexcodes;
        private ColorPalette _colorPalette;
        private bool _pinnedPalette = false;
        private bool _showWebsites = false;
        private List<Color> _pinnedColorList = new List<Color>();
        
        public override void OnInspectorGUI()
        {
            HeaderSection();
            PaletteSettingsSection();
            
            EditorGUILayout.Space(10f);
            if (_colorPalette.settings != null)
            {
                if (_colorPalette.colorProperties.Count != _colorPalette.settings.colorIds.Count)
                {
                    //If settings was updated, update palette
                    _colorPalette.UpdateProperties();
                }
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                DrawColorList();
                EditorGUILayout.EndVertical();
            }
            else
            {
                _colorPalette.colorProperties = new List<ColorProperty>();
                EditorUtility.SetDirty(_colorPalette);
                EditorGUILayout.HelpBox("A Palette Settings is required to generate the palette", MessageType.Error);
            }
            
            PinAndClipboardSection();
            WebsitesSection();
        }

        private void PaletteSettingsSection()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("This holds the palette settings of your project", ColorAssistantUtils.GUIMessageStyle);
            var settingsProperty = serializedObject.FindProperty("settings");
            EditorGUILayout.PropertyField(settingsProperty);
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.EndVertical();
        }
        private void PinAndClipboardSection()
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.Space();
            GUILayout.Label("Clipboard", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            GUILayout.Label("Copied Color Palettes will show up here",
                ColorAssistantUtils.GUIMessageStyle);
            DrawPinnedColors();
            DrawColorsFromClipboard();
            EditorGUILayout.EndVertical();
        }
        private static void HeaderSection()
        {
            ColorAssistantUtils.DrawHeader();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Create a palette for a certain palette settings", ColorAssistantUtils.GUIMessageStyle);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }
        private void DrawColorsFromClipboard()
        {
            if (string.IsNullOrEmpty(GUIUtility.systemCopyBuffer)) return;
            const string pattern = @"#[0-9\a-f][0-9\a-f][0-9\a-f][0-9\a-f][0-9\a-f][0-9\a-f]";
            var rg = new Regex(pattern);
            var matchedHexCodes = rg.Matches(GUIUtility.systemCopyBuffer);
            var refinedMatchedHexCodes = new List<string>();
            foreach (Match matchedHexCode in matchedHexCodes)
            {
                if(!refinedMatchedHexCodes.Contains(matchedHexCode.Value))
                    refinedMatchedHexCodes.Add(matchedHexCode.Value);
            }
            
            if (refinedMatchedHexCodes.Count < 2)
            {
                return;
            }
            //If not pinned then provide option to pin
            if(!_pinnedPalette)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("Colors from Clipboard");
                EditorGUILayout.LabelField("You can copy these colors to main palette only after you pin them");
                if (GUILayout.Button("Pin the colors"))
                {
                    _pinnedPalette = true;
                    _pinnedColorList.Clear();
                    for (var i = 0; i < refinedMatchedHexCodes.Count; i++)
                    {
                        ColorUtility.TryParseHtmlString(refinedMatchedHexCodes[i], out var color);
                        _pinnedColorList.Add(color);
                    }
                }
                EditorGUI.BeginDisabledGroup(true);
                for (var i = 0; i < refinedMatchedHexCodes.Count; i++)
                {
                    ColorUtility.TryParseHtmlString(refinedMatchedHexCodes[i], out var color);
                    DrawCopiedColorField(i, color);
                }
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndVertical();
            }
        }
        private void DrawPinnedColors()
        {
            if (_pinnedPalette)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("Pinned Colors");
                for (int i = 0; i < _pinnedColorList.Count; i++)
                {
                    DrawCopiedColorField(i, _pinnedColorList[i], true);
                }

                if (GUILayout.Button("Copy all colors to main palette"))
                {
                    if (EditorUtility.DisplayDialog("Overwrite main palette?",
                        "Are you sure you want to copy all colors from pinned to the main palette? This will overwrite " +
                        "the main palette colors on the respective indices. This can not be undone",
                        "Overwrite",
                        "Cancel"))
                    {
                        var count = Mathf.Min(_colorPalette.colorProperties.Count, _pinnedColorList.Count);
                        for (var i = 0; i < count; i++)
                            _colorPalette.colorProperties[i].color = _pinnedColorList[i];

                        EditorUtility.SetDirty(_colorPalette);
                    }
                }
                if (GUILayout.Button("Remove from pinned"))
                {
                    _pinnedPalette = false;
                    _pinnedColorList.Clear();
                }
                EditorGUILayout.EndVertical();
            }
        }
        private void DrawCopiedColorField(int i, Color color, bool canBeCopied = false)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.ColorField("", color);
            if (canBeCopied)
            {
                if (GUILayout.Button("Copy"))
                {
                    GUIUtility.systemCopyBuffer = "#" + ColorUtility.ToHtmlStringRGBA(color);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        private void DrawColorList()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Main Color Palette", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            for (var i = 0; i < _colorPalette.colorProperties.Count; i++)
                DrawColorField(i, _colorPalette.colorProperties[i].colorId, _colorPalette.colorProperties[i].color);                
            Undo.RecordObject(_colorPalette, "Color Palette");

            if (GUILayout.Button("Copy Color Palette"))
            {
                var colorString = "";
                for (var i = 0; i < _colorPalette.colorProperties.Count; i++)
                {
                    colorString += "[#";
                    colorString += ColorUtility.ToHtmlStringRGB(_colorPalette.colorProperties[i].color);
                    colorString += "], ";
                }

                EditorGUIUtility.systemCopyBuffer = colorString;
            }
            EditorGUILayout.EndVertical();
        }
        private void DrawColorField(int index, string colorName, Color color)
        {
            EditorGUILayout.BeginHorizontal();
            
            _colorPalette.colorProperties[index].color = EditorGUILayout.ColorField(colorName, color);
            var hasColorInClipboard = ColorUtility.TryParseHtmlString(GUIUtility.systemCopyBuffer, out var copiedColor);
            
            EditorGUI.BeginDisabledGroup(!hasColorInClipboard);
            if (GUILayout.Button("Paste"))
            {
                Undo.RecordObject(_colorPalette, "Color Palette");
                _colorPalette.colorProperties[index].color = copiedColor;
            }
            EditorGUI.EndDisabledGroup();
            
            EditorGUILayout.EndHorizontal();
            EditorUtility.SetDirty(_colorPalette);
        }

        private void WebsitesSection()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Helpful Websites", EditorStyles.boldLabel);
            GUILayout.Label("Create color palettes in preferred websites -> Export as CSS -> Copy CSS");
            EditorGUILayout.EndVertical();
            if (!_showWebsites)
                if (GUILayout.Button("Show Links")) _showWebsites = true;
            //GUILayout.Label("Helpful Websites", EditorStyles.boldLabel);
            if (_showWebsites)
            {
                EditorGUILayout.Space();
                ContentLink("(Adobe) Palette from Image", "https://color.adobe.com/create/image");
                ContentLink("(Adobe) Palette from Color Harmony", "https://color.adobe.com/create/color-wheel");
                ContentLink("(Coolors) Random Palette Generator", "https://coolors.co/bce7fd-c492b1-af3b6e-424651-21fa90");
                EditorGUILayout.Space();
                if (GUILayout.Button("Hide Links")) _showWebsites = false;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        private void ContentLink(string content, string link)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(content);
            if (GUILayout.Button("Link", EditorStyles.miniButtonRight, GUILayout.Width(100)))
            {
                Application.OpenURL(link);
            }
            EditorGUILayout.EndHorizontal();
        }
        private void OnEnable()
        {
            _colorPalette = (ColorPalette) target;
            _colorPalette.UpdateProperties();
        }
        private void OnDisable()
        {
            
        }
        private void OnDestroy()
        {
            
        }
        
    }
}