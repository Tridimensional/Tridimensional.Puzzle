using Tridimensional.Puzzle.Core.Entity;

namespace Tridimensional.Puzzle.Service.Contract
{
	public class SliceContract
	{
        public int Width { get; set; }
        public int Height { get; set; }
        public Point[,] Vertexes { get; set; }
        public Curve Lines { get; set; }
	}
}
