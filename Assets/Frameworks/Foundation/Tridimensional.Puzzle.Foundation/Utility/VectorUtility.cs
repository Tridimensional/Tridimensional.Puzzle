using UnityEngine;
using System;

namespace Tridimensional.Puzzle.Foundation.Utility
{
	public class VectorUtility
	{
        public static float GetAngle(Vector2 from, Vector2 to)
        {
            var divisor = Mathf.Sqrt((from.x * from.x + from.y * from.y) * (to.x * to.x + to.y * to.y));
            if (divisor == 0) { return 0; }

            var sin = (from.x * to.y - from.y * to.x) / divisor;
            var cos = (from.x * to.x + from.y * to.y) / divisor;

            return (sin >= 0 ? Mathf.Acos(cos) : (2 * Mathf.PI - Mathf.Acos(cos))) * 180.0f / Mathf.PI;
        }

        public static bool OntheSameSide(Vector2 p1, Vector2 p2, Vector2 a, Vector2 b)
        {
            var A = b.y - a.y;
            var B = a.x - b.x;
            var C = -(a.y * B + a.x * A);

            return (A * p1.x + B * p1.y + C) * (A * p2.x + B * p2.y + C) >= 0;
        }

        public static Vector2[] Reverse(Vector2[] points)
        {
            if (points == null) { return null; }
            var result = new Vector2[points.Length];
            for (var i = 0; i < points.Length; i++) { result[i] = points[points.Length - i - 1]; }
            return result;
        }

        public static Vector2 GetInnerPoint(Vector2 previous, Vector2 origin, Vector2 next, float distance)
        {
            var from = previous - origin;
            var to = next - origin;
            return origin + distance * ((from / Mathf.Sqrt(from.x * from.x + from.y * from.y)) + (to / (Mathf.Sqrt(to.x * to.x + to.y * to.y))));
        }
    }
}
