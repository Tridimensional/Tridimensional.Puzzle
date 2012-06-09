using UnityEngine;

namespace Tridimensional.Puzzle.Core
{
	public class GlobalConfiguration
	{
        public static Color BackgroundColor { get { return new Color32(0xff, 0xff, 0xff, 0xff); } }
        public static float PictureHeightInMeter { get { return 0.5f; } }
        public static float PieceThicknessInMeter { get { return 0.005f; } }
        public static float CameraToSubjectInMeter { get { return 0.8f; } }
        public static int SoftenWidthInPixel { get { return 3; } }
	}
}
