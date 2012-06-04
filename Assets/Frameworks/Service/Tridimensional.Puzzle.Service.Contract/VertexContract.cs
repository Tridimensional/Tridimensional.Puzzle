using UnityEngine;
namespace Tridimensional.Puzzle.Service.Contract
{
	public class VertexContract
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public int Index { get; set; }
        public VertexContract Previous { get; set; }
        public VertexContract Next { get; set; }
	}
}
