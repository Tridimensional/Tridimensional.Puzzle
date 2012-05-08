using System.Collections.Generic;
using Tridimensional.Puzzle.Foundation.Entity;
using Tridimensional.Puzzle.Foundation.Enumeration;
using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
	public interface IModelService
	{
        LayoutContract GetProperLayout(float width, float height, int count);
        LayoutContract GetProperLayout(float width, float height, GameDifficulty gameDifficulty);
        LayoutContract GetProperLayout(Texture2D image, int count);
        LayoutContract GetProperLayout(Texture2D image, GameDifficulty gameDifficulty);
        SliceContract GetSlice(LayoutContract layoutContract, SliceProgram sliceProgram);
        PieceContract[,] GeneratePiece(SliceContract sliceContract, Texture2D image);
    }
}

