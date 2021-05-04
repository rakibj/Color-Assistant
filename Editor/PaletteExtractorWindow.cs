using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace com.rakib.colorassistant
{
    public class PaletteExtractorWindow : EditorWindow
    {
        private Texture2D _texture;
        private List<Bin> _bins = new List<Bin>();
        private List<Color> _palette = new List<Color>();
        private Texture2D _binCreatedTex;

        [MenuItem("Rakib/Palette Extractor")]
        private static void ShowWindow()
        {
            var window = GetWindow<PaletteExtractorWindow>();
            window.titleContent = new GUIContent("Palette Extractor");
            window.Show();
        }

        private void OnGUI()
        {
            ColorAssistantUtils.DrawHeader();
            
            //Get Resize and draw texture
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            var buttonText = _texture == null ? "Import Image" : "Replace Image";
            if (GUILayout.Button(buttonText))
            {
                _texture = ColorAssistantUtils.GetTextureFromExplorer();
                _texture = ColorAssistantUtils.RescaleTexture(_texture, 100, true);
                _binCreatedTex = null;
            }
            if(_texture) ColorAssistantUtils.DrawTexture(_texture);
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Process"))
            {
                //Create 10x10 Bins From Texture
                PopulateBins();

                //Euclidean distance between colors function
                //for each pixel, find its distance from all colors in palette
                
                //if distance > threshold1 ? 1 : 0
                //Create vote texture mask, vote = black
                //Sum all votes of each bin
                //Get the highest voted bin
                //Calculate Sp of each pixel in the bin (Sp = number of voted neighbors in 20x20 dimension)
                //Continue while highest vote is less than threshold2
            }

            if (_binCreatedTex)
            {
                GUILayout.Label("Create 10x10 Bins From Texture");
                ColorAssistantUtils.DrawTexture(_binCreatedTex);
            }
            
        }

        private void PopulateBins()
        {
            var binDimension = 10;
            var horizontalBins = Mathf.Ceil(_texture.width / (float) binDimension);
            var verticalBins = Mathf.Ceil(_texture.height / (float) binDimension);
            _bins.Clear();

            for (int i = 0; i < horizontalBins; i++)
            {
                for (int j = 0; j < verticalBins; j++)
                {
                    var binStartX = i * binDimension;
                    var binStartY = j * binDimension;
                    var bin = new Bin(binDimension, new Rect(binStartX, binStartY, binDimension, binDimension));
                    _bins.Add(bin);
                    bin.LogBin();
                }
            }

            if (_binCreatedTex) DestroyImmediate(_binCreatedTex);
            _binCreatedTex = Instantiate(_texture);
            foreach (var bin in _bins)
            {
                for (int i = (int) bin.rect.x; i < bin.rect.x + bin.rect.width; i++)
                {
                    for (int j = (int) bin.rect.y; j < bin.rect.y + bin.rect.height; j++)
                    {
                        if (i == (int) bin.rect.x || j == (int) bin.rect.y)
                            _binCreatedTex.SetPixel(i, j, Color.red);
                    }
                }
            }

            _binCreatedTex.Apply();
        }

        private float Distance(Color c1, Color c2)
        {
            var rSq = c1.r * 256 - c2.r * 256;
            var gSq = c1.g * 256 - c2.g * 256;
            var bSq = c1.b * 256 - c2.b * 256;
            rSq *= rSq;
            gSq *= gSq;
            bSq *= bSq;
            return Mathf.Sqrt(rSq + gSq + bSq);
        }
    }
}