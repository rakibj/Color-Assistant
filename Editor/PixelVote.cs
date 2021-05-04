using UnityEngine;

namespace com.rakib.colorassistant
{
    public class PixelVote
    {
        public Color pixel;
        public int vote;
    }

    public class Bin
    {
        public int dimension;
        public Rect rect;
        public PixelVote[,] pixelVotes;

        public Bin(int dimension, Rect rect)
        {
            this.dimension = dimension;
            this.rect = rect;
            pixelVotes = new PixelVote[dimension, dimension];
        }

        public void LogBin()
        {
            Debug.Log("Bin Rect: " + rect);
        }
    }
}