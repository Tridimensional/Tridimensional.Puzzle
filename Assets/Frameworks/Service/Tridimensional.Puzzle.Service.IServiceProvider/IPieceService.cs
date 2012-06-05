using Tridimensional.Puzzle.Service.Contract;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
	public interface IPieceService
	{
        PieceContract[,] GeneratePiece(SliceContract sliceContract);
    }
}
