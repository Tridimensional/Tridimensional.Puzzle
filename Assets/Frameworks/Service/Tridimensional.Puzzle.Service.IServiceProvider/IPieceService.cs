using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
	public interface IPieceService
	{
        PieceContract[,] GeneratePiece(SliceContract sliceContract, Vector2 mappingOffset);
    }
}
