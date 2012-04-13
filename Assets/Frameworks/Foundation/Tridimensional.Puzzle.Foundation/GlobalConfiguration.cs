using Tridimensional.Puzzle.Service.ServiceImplementation;
using UnityEngine;

namespace Tridimensional.Puzzle.Foundation
{
	public class GlobalConfiguration
	{
        public static Color BackgroundColor { get { return new Color32(0xff, 0xff, 0xff, 0xff); } }
        public static float PictureRangeInMeter { get { return 0.85f; } }
	}
}
