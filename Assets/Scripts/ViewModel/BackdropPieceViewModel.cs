using UnityEngine;

namespace Tridimensional.Puzzle.UI.ViewModel
{
    public class BackdropPieceViewModel
    {
        public Vector3 Position { get; set; }
        public Mesh MappingMesh { get; set; }
        public Mesh BackseatMesh { get; set; }
        public float Distance { get; set; }
    }
}
