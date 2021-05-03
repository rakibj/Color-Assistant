using System;
using System.Collections.Generic;
using System.IO;
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
        private float _tolerance = 0.1f;

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
                GetTextureFromExplorer();
            if(_texture) ResizeAndDrawTexture();
            EditorGUILayout.EndVertical();

            if (_texture)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                _tolerance = EditorGUILayout.Slider("Tolerance Filter",_tolerance, 0.1f, 1f);
                if (GUILayout.Button("Process"))
                {
                    CreateTempTexture();
                    ProcessTempTexture();
                }
                EditorGUILayout.EndHorizontal();
                ProcessAndDrawPalette();
                EditorGUILayout.EndVertical();
                //DestroyImmediate(_tempTex);
            }
        }

        private void ProcessAndDrawPalette()
        {
            if (_filteredColors != null && _filteredColors.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Gradient (" + _filteredColors.Count + " colors)");
                var targetWidth = 200f;
                _paletteWidth = Mathf.FloorToInt(targetWidth / (float) _filteredColors.Count) * _filteredColors.Count;
                _paletteTex = new Texture2D(_paletteWidth, 1);
                var colors = new Color[_paletteWidth];
                var bandWidth = Mathf.FloorToInt((float) _paletteWidth / (float) _filteredColors.Count);
                for (int i = 0; i < _filteredColors.Count; i++)
                {
                    for (int j = 0; j < bandWidth; j++)
                    {
                        colors[i * bandWidth + j] = _filteredColors[i];
                    }
                }

                _paletteTex.SetPixels(colors);
                _paletteTex.Apply();
                var paletteTexRect = GUILayoutUtility.GetRect(_paletteTex.width, 20, GUILayout.ExpandWidth(false),
                    GUILayout.ExpandHeight(false));
                EditorGUI.DrawPreviewTexture(paletteTexRect, _paletteTex);
                EditorGUILayout.EndHorizontal();
            }
        }
        private void ProcessTempTexture()
        {
            var pixelColors = new List<Color>();
            _filteredColors = new List<Color>();
            pixelColors = LoadPixelList(pixelColors);
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
        private List<Color> LoadPixelList(List<Color> pixelList)
        {
            for (var i = 0; i < _tempTex.width; i++)
            {
                for (var j = 0; j < _tempTex.height; j++)
                {
                    var pixel = _tempTex.GetPixel(i, j);
                    pixelList.Add(pixel);
                }
            }

            return pixelList;
        }
        private void CreateTempTexture()
        {
            _tempTex = new Texture2D(_texture.width, _texture.height, _texture.format, false);
            _tempTex.SetPixels(_texture.GetPixels());
            _tempTex.Apply();
        }
        private void ResizeAndDrawTexture()
        {
            var resolutionFactor = (float) _maxHeight / _texture.height;
            _width = Mathf.FloorToInt(_texture.width * resolutionFactor);
            _texRect = GUILayoutUtility.GetRect(_width, _maxHeight, GUILayout.ExpandWidth(false),
                GUILayout.ExpandHeight(false));
            EditorGUI.DrawPreviewTexture(_texRect, _texture);
        }
        private void GetTextureFromExplorer()
        {
            string path = EditorUtility.OpenFilePanel("Select a png", "", "png");
            if (path.Length != 0)
            {
                _texture = new Texture2D(500, 500);
                var fileContent = File.ReadAllBytes(path);
                _texture.LoadImage(fileContent);
            }
            
        }
    }
}