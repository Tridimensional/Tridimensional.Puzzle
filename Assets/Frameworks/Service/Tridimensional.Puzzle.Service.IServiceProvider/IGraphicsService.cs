using System;
using Tridimensional.Puzzle.Core.Entity;
using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
    public interface IGraphicsService
	{
        Texture2D GenerateNormalMap(SliceContract sliceContract);
        Texture2D GenerateNormalMap(SliceContract sliceContract, float strength);
        Texture2D GenerateHeightMap(SliceContract sliceContract);
    }
}
