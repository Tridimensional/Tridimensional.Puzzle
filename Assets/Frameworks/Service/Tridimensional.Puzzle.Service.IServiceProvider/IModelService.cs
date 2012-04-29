using System.Collections.Generic;
using Tridimensional.Puzzle.Foundation.Entity;
using Tridimensional.Puzzle.Foundation.Enumeration;
using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
	public interface IModelService
	{
        LayoutContract GetProperLayout(int width, int height, int count);
        LayoutContract GetProperLayout(int width, int height, GameDifficulty gameDifficulty);
        SliceContract GetSlice(LayoutContract layoutContract, SliceProgram sliceProgram);
        MeshContract[,] GenerateMesh(SliceContract sliceContract, Texture2D image);
    }
}

