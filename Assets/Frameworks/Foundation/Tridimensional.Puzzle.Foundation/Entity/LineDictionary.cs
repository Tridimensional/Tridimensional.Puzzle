using System.Collections.Generic;
using UnityEngine;

namespace Tridimensional.Puzzle.Foundation.Entity
{
    public class LineDictionary
    {
        Dictionary<string, Vector2[]> _dictionary = new Dictionary<string, Vector2[]>();

        public Vector2[] this[int x1, int y1, int x2, int y2]
        {
            get { return Get(x1, y1, x2, y2); }
            set { _dictionary[GenerateKey(x1, y1, x2, y2)] = value; }
        }

        public Vector2[] Get(int x1, int y1, int x2, int y2)
        {
            var key = GenerateKey(x1, y1, x2, y2);
            return _dictionary[key];
        }

        public static string GenerateKey(int x1, int y1, int x2, int y2)
        {
            return string.Format("{0},{1},{2},{3}", x1, y1, x2, y2);
        }
    }
}
