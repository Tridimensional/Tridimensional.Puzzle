using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
	public interface IMeshService
	{
        Mesh[,] GenerateMesh(SliceContract sliceContract);
    }
}
