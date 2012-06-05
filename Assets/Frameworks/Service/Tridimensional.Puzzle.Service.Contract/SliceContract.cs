using Tridimensional.Puzzle.Foundation.Entity;

namespace Tridimensional.Puzzle.Service.Contract
{
	public class SliceContract
	{
        public int Width { get; set; }
        public int Height { get; set; }
        public Point[,] Vertexes { get; set; }
        public LineDictionary Lines { get; set; }
	}
}
