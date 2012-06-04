using Tridimensional.Puzzle.Foundation.Entity;

namespace Tridimensional.Puzzle.Service.Contract
{
	public class SliceContract
	{
        public Point[,] Vertexes { get; set; }
        public LineDictionary Lines { get; set; }
	}
}
