using System;
using Tridimensional.Puzzle.Service.Contract;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
	public interface IPieceService
	{
        PieceContract[,] GeneratePieceContracts(SliceContract sliceContract);
        PieceContract[,] GeneratePieceContracts(SliceContract sliceContract, Action pieceCompleted);
    }
}
