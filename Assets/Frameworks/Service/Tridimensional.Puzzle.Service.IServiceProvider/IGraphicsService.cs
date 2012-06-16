using System;
using Tridimensional.Puzzle.Core.Entity;
using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
    public interface IGraphicsService
	{
        Texture2D GenerateNormalMap(SliceContract sliceContract);
        Texture2D GenerateNormalMap(SliceContract sliceContract, Action<float> percentComplet);
        Texture2D GenerateNormalMap(SliceContract sliceContract, float strength);
        Texture2D GenerateNormalMap(SliceContract sliceContract, float strength, Action<float> percentComplet);
        Texture2D GenerateHeightMap(SliceContract sliceContract);
        Texture2D GenerateHeightMap(SliceContract sliceContract, Action<float> percentComplet);
    }
}
