using UnityEngine;

namespace Tridimensional.Puzzle.Core
{
	public class GlobalConfiguration
	{
        public static Color BackgroundColor = new Color32(0xff, 0xff, 0xff, 0xff);
        public static float PictureHeightInMeter = 0.5f;
        public static float PieceThicknessInMeter = 0.005f;
        public static float CameraToSubjectInMeter = 0.8f;
        public static float SurfaceThicknessInMeter = 0.02f;
        public static float DeskThinknessInMeter = 0.5f;
        public static int SoftenWidthInPixel = 3;
        public static Vector3 DeskPosition = new Vector3(0, 0, 0);
    }
}
