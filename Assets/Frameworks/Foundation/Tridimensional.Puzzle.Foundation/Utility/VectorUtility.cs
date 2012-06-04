using System;
using Tridimensional.Puzzle.Foundation.Entity;
using UnityEngine;

namespace Tridimensional.Puzzle.Foundation.Utility
{
	public class VectorUtility
	{
        public static float GetAngle(Point from, Point to)
        {
            return GetAngle(from.X, from.Y, to.X, to.Y);
        }

        public static float GetAngle(Vector2 from, Vector2 to)
        {
            return GetAngle(from.x, from.y, to.x, to.y);
        }

        public static float GetAngle(float x1, float y1, float x2, float y2)
        {
            var divisor = Mathf.Sqrt(1f * (x1 * x1 + y1 * y1) * (x2 * x2 + y2 * y2));
            if (divisor == 0) { return 0; }

            var sin = (x1 * y2 - y1 * x2) / divisor;
            var cos = (x1 * x2 + y1 * y2) / divisor;

            return (sin >= 0 ? Mathf.Acos(cos) : (2 * Mathf.PI - Mathf.Acos(cos))) * 180.0f / Mathf.PI;
        }

        public static bool OntheSameSide(Vector2 p1, Vector2 p2, Vector2 a, Vector2 b)
        {
            var A = b.y - a.y;
            var B = a.x - b.x;
            var C = -(a.y * B + a.x * A);

            return (A * p1.x + B * p1.y + C) * (A * p2.x + B * p2.y + C) >= 0;
        }

        public static Point[] Reverse(Point[] points)
        {
            if (points == null) { return null; }
            var result = new Point[points.Length];
            for (var i = 0; i < points.Length; i++) { result[i] = points[points.Length - i - 1]; }
            return result;
        }
    }
}
