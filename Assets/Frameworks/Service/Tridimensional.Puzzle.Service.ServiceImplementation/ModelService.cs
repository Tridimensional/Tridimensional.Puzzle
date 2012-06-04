using System;
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
        IPieceService _pieceService;
        SliceStrategyFactory _sliceStrategyFactory;

        public ModelService(IPieceService pieceService, SliceStrategyFactory sliceStrategyFactory)
        {
            _pieceService = pieceService;
            _sliceStrategyFactory = sliceStrategyFactory;
        }

        public LayoutContract GetProperLayout(Texture2D image, GameDifficulty gameDifficulty)
        {
            return GetProperLayout(image.width, image.height, gameDifficulty);
        }

        public LayoutContract GetProperLayout(Texture2D image, int count)
        {
            return GetProperLayout(image.width, image.height, count);
        }

        public LayoutContract GetProperLayout(int width, int height, GameDifficulty gameDifficulty)
        {
            var puzzleCount = gameDifficulty.ToProperPuzzleCount();
            return GetProperLayout(width, height, puzzleCount);
        }

        public LayoutContract GetProperLayout(int width, int height, int count)
        {
            var rows = Mathf.Sqrt(1.0f * height / width * count);
            var columns = rows * width / height;

            return new LayoutContract
            {
                Rows = (int)Math.Ceiling(rows),
                Columns = (int)Math.Ceiling(columns),
                Height = height,
                Width = width
            };
        }

        public SliceContract GetSlice(LayoutContract layoutContract, SliceProgram sliceProgram)
        {
            var sliceStrategy = _sliceStrategyFactory.Create(sliceProgram);
            return sliceStrategy.GetSlice(layoutContract);
        }

        public PieceContract[,] GeneratePiece(SliceContract sliceContract, Texture2D image)
        {
            return _pieceService.GeneratePiece(sliceContract, image);
        }

        public Texture2D GenerateNormalMap(SliceContract sliceContract, Texture2D image)
        {
            return null;
        }
    }
}
