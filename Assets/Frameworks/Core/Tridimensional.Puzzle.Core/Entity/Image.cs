using System;
using UnityEngine;

namespace Tridimensional.Puzzle.Core.Entity
{
	public class Image
	{
        public int width { get; set; }
        public int height { get; set; }

        Color[,] _colors;

        public Image(int width, int height)
        {
            this.width = width;
            this.height = height;
            _colors = new Color[width, height];
        }

        public Color GetPixel(int i, int j)
        {
            try { return _colors[i, j]; }
            catch (IndexOutOfRangeException) { return Color.clear; }
        }

        public void SetPixel(int i, int j, Color color)
        {
            try { _colors[i, j] = color; }
            catch (IndexOutOfRangeException) { }
        }
    }
}
