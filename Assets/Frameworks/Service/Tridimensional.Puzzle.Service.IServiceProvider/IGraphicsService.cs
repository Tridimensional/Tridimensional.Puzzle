using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
    public interface IGraphicsService
	{
        Texture2D GenerateNormalMap(SliceContract sliceContract);
    }
}
