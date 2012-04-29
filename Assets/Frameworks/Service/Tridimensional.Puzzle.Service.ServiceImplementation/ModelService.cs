using System;
using Tridimensional.Puzzle.Foundation;
using Tridimensional.Puzzle.Foundation.Enumeration;
using Tridimensional.Puzzle.Service.Contract;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation.SliceStrategy;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation
{
    public class ModelService : IModelService
    {
        IMeshService _meshService;
        SliceStrategyFactory _sliceStrategyFactory;

        public ModelService(IMeshService meshService, SliceStrategyFactory sliceStrategyFactory)
        {
            _meshService = meshService;
            _sliceStrategyFactory = sliceStrategyFactory;
        }

        public LayoutContract GetProperLayout(int width, int height, GameDifficulty gameDifficulty)
        {
            var puzzleCount = gameDifficulty.ToProperPuzzleCount();
            return GetProperLayout(width, height, puzzleCount);
        }

        public LayoutContract GetProperLayout(int width, int height, int count)
        {
            var rows = Math.Sqrt(1.0 * height / width * count);
            var columns = rows * width / height;
            var layoutContract = new LayoutContract();

            layoutContract.Rows = (int)Math.Ceiling(rows);
            layoutContract.Columns = (int)Math.Ceiling(columns);

            if (width > height)
            {
                layoutContract.Width = GlobalConfiguration.PictureRangeInMeter;
                layoutContract.Height = layoutContract.Width * layoutContract.Rows / layoutContract.Columns;
            }
            else
            {
                layoutContract.Height = GlobalConfiguration.PictureRangeInMeter;
                layoutContract.Width = layoutContract.Height * layoutContract.Columns / layoutContract.Rows;
            }

            return layoutContract;
        }

        public SliceContract GetSlice(LayoutContract layoutContract, SliceProgram sliceProgram)
        {
            var sliceStrategy = _sliceStrategyFactory.Create(sliceProgram);
            return sliceStrategy.GetSlice(layoutContract);
        }

        public MeshContract[,] GenerateMesh(SliceContract sliceContract)
        {
            return _meshService.GenerateMesh(sliceContract);
        }
    }
}
