using Tridimensional.Puzzle.Service.ServiceImplementation;
using UnityEngine;

namespace Tridimensional.Puzzle.Foundation
{
	public class GlobalConfiguration
	{
        public static Color BackgroundColor { get { return new Color32(0xff, 0xff, 0xff, 0xff); } }
        public static float PictureHeightInMeter { get { return 0.5f; } }
        public static float PieceThicknessInMeter { get { return 0.0035f; } }
        public static float CameraToSubjectInMeter { get { return 0.8f; } }
        public static int SoftenWidthInPixel { get { return 3; } }
	}
}
