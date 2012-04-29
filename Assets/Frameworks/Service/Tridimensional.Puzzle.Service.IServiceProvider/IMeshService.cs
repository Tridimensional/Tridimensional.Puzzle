using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
	public interface IMeshService
	{
        MeshContract[,] GenerateMesh(SliceContract sliceContract);
    }
}
