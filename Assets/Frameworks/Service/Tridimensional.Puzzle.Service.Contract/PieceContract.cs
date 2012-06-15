using UnityEngine;

namespace Tridimensional.Puzzle.Service.Contract
{
	public class PieceContract
	{
        public Vector3 Position { get; set; }
        public MeshContract MappingMesh { get; set; }
        public MeshContract BackseatMesh { get; set; }
	}
}
