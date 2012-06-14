using System;
using Tridimensional.Puzzle.Core.Enumeration;
using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
    public interface IPuzzleService
	{
        LayoutContract GetProperLayout(int width, int height, int count);
        LayoutContract GetProperLayout(int width, int height, Difficulty difficulty);
        LayoutContract GetProperLayout(Texture2D image, int count);
        LayoutContract GetProperLayout(Texture2D image, Difficulty difficulty);
        SliceContract GetSlice(Texture2D image, LayoutContract layoutContract, SlicePattern slicePattern);
        PieceContract[,] GeneratePieceContracts(SliceContract sliceContract);
        PieceContract[,] GeneratePieceContracts(SliceContract sliceContract, Action pieceCompleted);
        Texture2D GenerateNormalMap(SliceContract sliceContract);
    }
}
