using UnityEngine;

namespace Tridimensional.Puzzle.Foundation.Entity
{
	public static  class PointExtension
	{
        public static Vector2 ToVector2(this Point point, float rate)
        {
            return new Vector2(point.X * rate, point.Y * rate);
        }
	}
}
