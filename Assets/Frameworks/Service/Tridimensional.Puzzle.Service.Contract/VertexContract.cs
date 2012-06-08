using UnityEngine;

namespace Tridimensional.Puzzle.Service.Contract
{
	public class VertexContract
    {
        public int Index { get; set; }
        public Vector3 Position { get; set; }
        public VertexContract Previous { get; set; }
        public VertexContract Next { get; set; }
	}
}
