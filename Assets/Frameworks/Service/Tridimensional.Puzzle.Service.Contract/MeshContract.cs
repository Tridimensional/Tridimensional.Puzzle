using UnityEngine;

namespace Tridimensional.Puzzle.Service.Contract
{
	public class MeshContract
	{
        public Vector3[] Vertices { get; set; }
        public int[] Triangles { get; set; }
        public Vector2[] Uv { get; set; }
    }
}
