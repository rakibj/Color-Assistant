using UnityEngine;

namespace com.rakib.colorassistant
{
    public class PixelVote
    {
        public Color pixel;
        public int vote;

        public PixelVote(Color pixel, int vote)
        {
            this.pixel = pixel;
            this.vote = vote;
        }
    }
    
    public class Bin
    {
        public int dimension;
        public Rect rect;
        public PixelVote[,] pixelVotes;
        private int[,] _pixelSps;
        private int _totalVotes;
        public int TotalVotes => _totalVotes;

        public Bin(){}
        public Bin(int dimension, Rect rect)
        {
            this.dimension = dimension;
            this.rect = rect;
            pixelVotes = new PixelVote[dimension, dimension];
            _pixelSps = new int[dimension, dimension];
        }

        public void LogBin()
        {
            Debug.Log("Bin Rect: " + rect);
        }

        public void CalculateTotalVotes()
        {
            _totalVotes = 0;
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    _totalVotes += pixelVotes[i, j].vote;
                }
            }
            //Debug.Log("Bin Rect: " + rect + " Total Votes: " + _totalVotes);
        }

        public void CalculatePixelSps(int neighbors)
        {
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    var sp = 0;
                    var xMin = Mathf.Max(i - neighbors, 0);
                    var xMax = Mathf.Min(i + neighbors, dimension - 1);
                    var yMin = Mathf.Max(j - neighbors, 0);
                    var yMax = Mathf.Min(j + neighbors, dimension - 1);
                    for (int k = xMin; k < xMax; k++)
                    {
                        for (int l = yMin; l < yMax; l++)
                        {
                            if (pixelVotes[k, l].vote == 1)
                                sp++;
                        }
                    }

                    //Debug.Log("Adding on " + i + ", " + j + "   sp: " + sp);
                    _pixelSps[i, j] = sp;
                }
            }
        }
        public (Color, int) GetPixelMaxSp()
        {
            var maxSp = -Mathf.Infinity;
            var pixel = Color.black;
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    if (_pixelSps[i, j] > maxSp)
                    {
                        maxSp = _pixelSps[i, j];
                        pixel = pixelVotes[i, j].pixel;
                    }
                }
            }

            return (pixel, (int)maxSp);
        }
    }
}