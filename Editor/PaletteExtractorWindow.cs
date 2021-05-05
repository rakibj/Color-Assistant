using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace com.rakib.colorassistant
{
    public class PaletteExtractorWindow : EditorWindow
    {
        private float _threshold1 = 0.45f;
        private float _threshold2;
        private Texture2D _texture;
        private List<Bin> _bins = new List<Bin>();
        private List<Color> _palette = new List<Color>();
        private Texture2D _binCreatedTex;
        private Texture2D _voteTex;
        private Bin _maxVotedBin = new Bin();
        private int _maxSp;
        private Color _pixelMaxSp;

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
                _voteTex = null;
            }
            if(_texture) ColorAssistantUtils.DrawTexture(_texture);
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Process"))
            {
                _threshold1 = 0.45f;
                _threshold2 = (_texture.width * _texture.height) / 1000f;
                    
                //Create 10x10 Bins From Texture
                PopulateBins();

                //Euclidean distance between colors function
                //Debug.Log("Distance between red and green: " + Distance(Color.red, Color.green));
                
                //Add a random color to the palette
                if(_palette.Count == 0) _palette.Add(_texture.GetPixel(0,0));
                
                //for each pixel, find its distance from all colors in palette
                //if distance > threshold1 ? 1 : 0
                //Create vote texture mask, vote = black
                CalculateVotes();

                //Sum all votes of each bin
                foreach (var bin in _bins)
                    bin.CalculateTotalVotes();
                
                //Get the highest voted bin
                _maxVotedBin = _bins.OrderByDescending(b => b.TotalVotes).First();

                //Calculate Sp of each pixel in the bin (Sp = number of voted neighbors in 20x20 dimension)
                _maxVotedBin.CalculatePixelSps(20);
                (_pixelMaxSp, _maxSp) = _maxVotedBin.GetPixelMaxSp();
                //Debug.Log("pixelMapSp: " + _pixelMaxSp + "  maxSp: " + _maxSp + "   _threshold2: " + _threshold2);

                //Continue while highest vote is less than threshold2
                if(_maxSp > _threshold2)
                    _palette.Add(_pixelMaxSp);
            }

            if (GUILayout.Button("Clear Data"))
            {
                _palette.Clear();
            }

            GUILayout.Label("Threshold 1: " + _threshold1);
            GUILayout.Label("Threshold 2: " + _threshold2.ToString("F1"));

            if (_binCreatedTex)
            {
                GUILayout.Label("1. Create 10x10 Bins From Texture");
                ColorAssistantUtils.DrawTexture(_binCreatedTex);
            }

            if (_voteTex)
            {
                GUILayout.Label("2. Populated votes");
                ColorAssistantUtils.DrawTexture(_voteTex);
            }
            GUILayout.Label("3. Max voted bin total votes: " + _maxVotedBin.TotalVotes);
            GUILayout.Label("4. Max sp: " + _maxSp);
            
            ColorAssistantUtils.DrawColorPalette(_palette, 200, 20);
        }

        private void CalculateVotes()
        {
            if (_voteTex) DestroyImmediate(_voteTex);
            _voteTex = Instantiate(_texture);

            

            foreach (var bin in _bins)
            {
                int k = 0;
                for (int i = (int) bin.rect.x; i < bin.rect.x + bin.rect.width; i++)
                {
                    int l = 0;
                    for (int j = (int) bin.rect.y; j < bin.rect.y + bin.rect.height; j++)
                    {
                        var pixel = _texture.GetPixel(i, j);
                        var vote = 1;
                        foreach (var paletteColor in _palette)
                        {
                            var distance = Distance(paletteColor, pixel);
                            if (distance < _threshold1)
                            {
                                vote = 0;
                                //Debug.Log("distance < threshold1 Distance: " + distance + 
                                //          "Threshold1: " + _threshold1 + "Vote: " + vote);
                                break;
                            }
                        }

                        var pixelVote = new PixelVote(pixel, vote);
                        if(k < bin.dimension && l < bin.dimension)
                            bin.pixelVotes[k, l] = pixelVote;
                        
                        
                        _voteTex.SetPixel(i, j, Color.white * vote);
                        l++;
                    }

                    k++;
                }
            }
            _voteTex.Apply();
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
                    //bin.LogBin();
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
            var multiplyFactor = 1f;
            var rSq = c1.r * multiplyFactor - c2.r * multiplyFactor;
            var gSq = c1.g * multiplyFactor - c2.g * multiplyFactor;
            var bSq = c1.b * multiplyFactor - c2.b * multiplyFactor;
            rSq *= rSq;
            gSq *= gSq;
            bSq *= bSq;
            return Mathf.Sqrt(rSq + gSq + bSq);
        }
    }
}