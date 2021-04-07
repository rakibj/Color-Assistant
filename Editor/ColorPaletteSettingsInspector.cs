using System;
using UnityEditor;
using UnityEngine;

namespace com.rakib.colorassistant
{
    [CustomEditor(typeof(ColorPaletteSettings))]
    public class ColorPaletteSettingsInspector: Editor
    {
        public override void OnInspectorGUI()
        {
            ColorAssistantUtils.DrawHeader();
            
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.Space();
            HeaderSection();
            
            serializedObject.Update();
            var colorIdsProperty = serializedObject.FindProperty("colorIds");
            EditorList.Show("Color Keys", "Key", colorIdsProperty);
            serializedObject.ApplyModifiedProperties();
            
            EditorGUILayout.EndVertical();

        }

        private static void HeaderSection()
        {
            GUILayout.Label("Palette Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label(
                "Define the color keys here. The key will link object with color. Palettes will be created using this " +
                "settings. Be careful with this as removing a key can break connections to the palette",
                ColorAssistantUtils.GUIMessageStyle);
            EditorGUILayout.EndVertical();
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }
    }
}