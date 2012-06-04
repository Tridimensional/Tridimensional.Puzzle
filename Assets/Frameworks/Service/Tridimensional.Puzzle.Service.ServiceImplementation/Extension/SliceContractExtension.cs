using Tridimensional.Puzzle.Foundation.Entity;

namespace Tridimensional.Puzzle.Service.Contract
{
	public static class SliceContractExtension
	{
        public static Point GetRange(this SliceContract sliceContract)
        {
            return sliceContract.Vertexes[sliceContract.Vertexes.GetLength(0) - 1, sliceContract.Vertexes.GetLength(1) - 1] - sliceContract.Vertexes[0, 0];
        }
	}
}
