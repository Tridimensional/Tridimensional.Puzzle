using System;
using Tridimensional.Puzzle.Core.Entity;
using Tridimensional.Puzzle.Service.Contract;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
    public interface IGraphicsService
	{
        Image GenerateNormalMap(SliceContract sliceContract);
        Image GenerateNormalMap(SliceContract sliceContract, Action<float> percentageCompleted);
        Image GenerateNormalMap(SliceContract sliceContract, float strength);
        Image GenerateNormalMap(SliceContract sliceContract, float strength, Action<float> percentageCompleted);
        Image GenerateHeightMap(SliceContract sliceContract);
        Image GenerateHeightMap(SliceContract sliceContract, Action<float> percentageCompleted);
    }
}
