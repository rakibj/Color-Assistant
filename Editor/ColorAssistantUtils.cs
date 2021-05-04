using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        public static Texture2D GetTextureFromExplorer()
        {
            string path = EditorUtility.OpenFilePanel("Select a png", "", "png");
            var texture = new Texture2D(500, 500);
            if (path.Length != 0)
            {
                var fileContent = File.ReadAllBytes(path);
                texture.LoadImage(fileContent);
            }
            return texture;
        }
        public static void DrawColorPalette(List<Color> paletteColors, int maxPaletteWidth, int paletteHeight)
        {
            if (paletteColors == null || paletteColors.Count <= 0) return;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Color Palette (" + paletteColors.Count + " colors)");
            var paletteWidth = Mathf.FloorToInt(maxPaletteWidth / (float) paletteColors.Count) * paletteColors.Count;
            var paletteTex = new Texture2D(paletteWidth, 1);
            var colors = new Color[paletteWidth];
            var bandWidth = Mathf.FloorToInt((float) paletteWidth / (float) paletteColors.Count);
            for (int i = 0; i < paletteColors.Count; i++)
            {
                for (int j = 0; j < bandWidth; j++)
                {
                    colors[i * bandWidth + j] = paletteColors[i];
                }
            }

            paletteTex.SetPixels(colors);
            paletteTex.Apply();
            var paletteTexRect = GUILayoutUtility.GetRect(paletteTex.width, paletteHeight, GUILayout.ExpandWidth(false),
                GUILayout.ExpandHeight(false));
            EditorGUI.DrawPreviewTexture(paletteTexRect, paletteTex);
            EditorGUILayout.EndHorizontal();
        }
        public static Texture2D GetCopyTexture(Texture2D texture)
        {
            var tempTex = new Texture2D(texture.width, texture.height, texture.format, false);
            tempTex.SetPixels(texture.GetPixels());
            tempTex.Apply();
            tempTex.Compress(false);
            return tempTex;
        }
        public static List<Color> GetColors(Texture2D texture)
        {
            var pixelList = new List<Color>();
            for (var i = 0; i < texture.width; i++)
            {
                for (var j = 0; j < texture.height; j++)
                {
                    var pixel = texture.GetPixel(i, j);
                    pixelList.Add(pixel);
                }
            }
            return pixelList;
        }
        public static (int, Rect) DrawTexture(Texture2D texture, int maxHeight)
        {
            var resolutionFactor = (float) maxHeight / texture.height;
            var width = Mathf.FloorToInt(texture.width * resolutionFactor);
            var texRect = GUILayoutUtility.GetRect(width, maxHeight, GUILayout.ExpandWidth(false),
                GUILayout.ExpandHeight(false));
            EditorGUI.DrawPreviewTexture(texRect, texture);
            return (width, texRect);
        }
        public static void DrawTexture(Texture2D texture)
        {
            var texRect = GUILayoutUtility.GetRect(texture.width, texture.height, GUILayout.ExpandWidth(false),
                GUILayout.ExpandHeight(false));
            EditorGUI.DrawPreviewTexture(texRect, texture);
        }
        public static Texture2D RescaleTexture(Texture2D texture, int maxHeight, bool makeSquare = false)
        {
            var heightResFactor = (float) maxHeight / texture.height;
            var widthResFactor = (float) maxHeight / texture.width;
            TextureScale.Bilinear(texture, Mathf.FloorToInt(texture.width * (makeSquare ? widthResFactor :heightResFactor)), 
                Mathf.FloorToInt(texture.height * heightResFactor));
            texture.Apply();
            Debug.Log("After rescale: height is " + texture.height);
            return texture;
        }
        public static Texture2D RescaleTexture(Texture2D texture, float scale)
        {
            TextureScale.Bilinear(texture, Mathf.FloorToInt(texture.width * scale), 
                Mathf.FloorToInt(texture.height * scale));
            texture.Apply();
            Debug.Log("After rescale: height is " + texture.height);
            return texture;
        }
    }
}