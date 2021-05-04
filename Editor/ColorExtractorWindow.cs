using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace com.rakib.colorassistant
{
    public class ColorExtractorWindow : EditorWindow
    {
        private Texture2D _texture;
        private Texture2D _tempTex;
        private int _maxHeight = 150;
        private int _width;
        private Rect _texRect;
        private int _paletteWidth;
        private Texture2D _paletteTex;
        private bool _drawTempTex = false;
        private List<Color> _filteredColors;
        private float _tolerance = 0.5f;

        /// <summary>
        /// Returns true if its considered a unique color
        /// </summary>
        /// <param name="filteredColor"></param>
        /// <param name="pixel"></param>
        delegate bool FilterDelegate(Color filteredColor, Color pixel);

        [MenuItem("Rakib/Color Extractor")]
        private static void ShowWindow()
        {
            var window = GetWindow<ColorExtractorWindow>();
            window.titleContent = new GUIContent("Color Extractor");
            //window.position = new Rect(0, 0, 400, 600);
            window.Show();
        }

        private void OnGUI()
        {
            ColorAssistantUtils.DrawHeader();
            
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            var buttonText = _texture == null ? "Import Image" : "Replace Image";
            if (GUILayout.Button(buttonText))
                _texture = ColorAssistantUtils.GetTextureFromExplorer();
            if (_texture)
                (_width, _texRect) = ColorAssistantUtils.DrawTexture(_texture, _maxHeight);
            EditorGUILayout.EndVertical();

            if (_texture)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                _tolerance = EditorGUILayout.Slider("Tolerance Filter",_tolerance, 0.1f, 1f);
                if (GUILayout.Button("Process"))
                {
                    _tempTex = ColorAssistantUtils.GetCopyTexture(_texture);
                    ProcessTempTexture();
                }
                EditorGUILayout.EndHorizontal();
                ColorAssistantUtils.DrawColorPalette(_filteredColors, 200, 20);
                EditorGUILayout.EndVertical();
            }
        }
        
        private void ProcessTempTexture()
        {
            var pixelColors = new List<Color>();
            _filteredColors = new List<Color>();
            pixelColors = ColorAssistantUtils.GetColors(_tempTex);
            _filteredColors = FilterPixels(pixelColors, ToleranceFilter);
            Debug.Log("Processing Complete! Colors Extracted: " + _filteredColors.Count);
        }
        private List<Color> FilterPixels(List<Color> filterFrom, FilterDelegate filter)
        {
            var filterTo = new List<Color>();
            for (var i = 0; i < filterFrom.Count; i++)
            {
                var pixel = filterFrom[i];
                var shouldAdd = true;
                for (var index = 0; index < filterTo.Count; index++)
                {
                    var filteredColor = filterTo[index];
                    if (filter(filteredColor, pixel)) continue;
                    shouldAdd = false;
                    break;
                }

                if (shouldAdd)
                {
                    filterTo.Add(pixel);
                }
            }

            return filterTo;
        }
        private bool ToleranceFilter(Color compareTo, Color pixel)
        {
            var unique = Math.Abs(compareTo.r - pixel.r) > _tolerance || Math.Abs(compareTo.g - pixel.g) > _tolerance || Math.Abs(compareTo.b - pixel.b) > _tolerance;
            return unique;
        }
        private bool EuclideanFilter(Color compareTo, Color pixel)
        {
            var rDiff = (compareTo.r - pixel.r) * 256;
            var gDiff = (compareTo.g - pixel.g) * 256;
            var bDiff = (compareTo.b - pixel.b) * 256;

            rDiff *= rDiff;
            gDiff *= gDiff;
            bDiff *= bDiff;
            
            var distanceSq = rDiff + gDiff + bDiff;
//            Debug.Log("distanceSq "+ distanceSq);
            var toleranceValue = 1.05f;
            return Math.Abs(distanceSq) > toleranceValue;
        }
    }
}