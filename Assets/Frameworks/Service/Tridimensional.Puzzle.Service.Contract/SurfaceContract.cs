using Tridimensional.Puzzle.Core.Enumeration;

namespace Tridimensional.Puzzle.Service.Contract
{
	public class SurfaceContract
	{
        public VertexContract[] Vertexes { get; set; }
        public Direction Direction { get; set; }
	}
}
