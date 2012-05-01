using UnityEngine;

namespace Tridimensional.Puzzle.Service.Contract
{
	public class PieceContract
	{
        public Vector3 Position { get; set; }
        public Mesh MappingMesh { get; set; }
        public Mesh BackseatMesh { get; set; }
	}
}
