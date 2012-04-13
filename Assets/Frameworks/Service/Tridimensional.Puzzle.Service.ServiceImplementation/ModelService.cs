using System;
using System.Collections.Generic;
using Tridimensional.Puzzle.Foundation;
using Tridimensional.Puzzle.Foundation.Entity;
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

        public FormationContract GetProperFormation(int width, int height, GameDifficulty gameDifficulty)
        {
            var puzzleCount = gameDifficulty.ToProperPuzzleCount();
            return GetProperFormation(width, height, puzzleCount);
        }

        public FormationContract GetProperFormation(int width, int height, int count)
        {
            var rows = Math.Sqrt(1.0 * height / width * count);
            var columns = rows * width / height;
            var formation = new FormationContract();

            formation.Rows = (int)Math.Ceiling(rows);
            formation.Columns = (int)Math.Ceiling(columns);

            if (width > height)
            {
                formation.Width = GlobalConfiguration.PictureRangeInMeter;
                formation.Height = formation.Width * formation.Rows / formation.Columns;
            }
            else
            {
                formation.Height = GlobalConfiguration.PictureRangeInMeter;
                formation.Width = formation.Height * formation.Columns / formation.Rows;
            }

            return formation;
        }

        public SliceContract GetSlice(FormationContract formationContract, SliceProgram sliceProgram)
        {
            var sliceStrategy = _sliceStrategyFactory.Create(sliceProgram);
            return sliceStrategy.GetSlice(formationContract);
        }

        public Mesh[,] GenerateMesh(SliceContract sliceContract)
        {
            return _meshService.GenerateMesh(sliceContract);
        }
    }
}
