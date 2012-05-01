using UnityEngine;

namespace Tridimensional.Puzzle.UI.ViewModel
{
	public class PieceViewModel
	{
        public GameObject Object { get; set; }
        public Vector3 Position { get; set; }
        public bool MovingCompleted { get; set; }
        public bool LandingCompleted { get; set; }
    }
}
