using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace com.rakib.colorassistant
{
    public static class ColorAssistantUtils
    {
        public static void DrawHeader()
        {
            EditorGUILayout.Space();
            var tex = 
                (Texture)AssetDatabase.LoadAssetAtPath("Packages/com.rakib.colorassistant/Textures/color_assistant.png", typeof(Texture));
            if(tex == null)
                tex = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Color-Assistant/Textures/color_assistant.png", typeof(Texture));
            ShowHeaderLogo(tex);
            EditorGUILayout.Space();
        }
        public static void ShowHeaderLogo(Texture tex)
        {
            var rect = GUILayoutUtility.GetRect(0f, 0f);
            rect.width = tex.width;
            rect.height = tex.height;
            GUILayout.Space(rect.height);
            GUI.DrawTexture(rect, tex);
        }

        public static GUIStyle GUIMessageStyle
        {
            get
            {
                var messageStyle = new GUIStyle(GUI.skin.label);
                messageStyle.wordWrap = true;
                return messageStyle;
            }
        }
    }
}