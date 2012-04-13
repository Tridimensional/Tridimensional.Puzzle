using System.Collections.Generic;
using Tridimensional.Puzzle.Foundation.Entity;
using Tridimensional.Puzzle.Foundation.Enumeration;
using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
	public interface IModelService
	{
        FormationContract GetProperFormation(int width, int height, int count);
        FormationContract GetProperFormation(int width, int height, GameDifficulty gameDifficulty);
        SliceContract GetSlice(FormationContract formationContract, SliceProgram sliceProgram);
        Mesh[,] GenerateMesh(SliceContract sliceContract);
    }
}

