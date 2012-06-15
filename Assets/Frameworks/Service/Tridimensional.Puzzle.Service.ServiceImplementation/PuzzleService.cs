using System;
using Tridimensional.Puzzle.Core.Entity;
using Tridimensional.Puzzle.Core.Enumeration;
using Tridimensional.Puzzle.Service.Contract;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation.SliceStrategy;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation
{
    public class PuzzleService : IPuzzleService
    {
        IPieceService _pieceService;
        IGraphicsService _graphicsService;
        SliceStrategyFactory _sliceStrategyFactory;

        #region Instance

        static IPuzzleService _instance;

        public static IPuzzleService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PuzzleService();
                }
                return _instance;
            }
        }

        #endregion

        public PuzzleService()
        {
            _pieceService = PieceService.Instance;
            _graphicsService = GraphicsService.Instance;
            _sliceStrategyFactory = SliceStrategyFactory.Instance;
        }

        public LayoutContract GetProperLayout(Texture2D image, Difficulty difficulty)
        {
            return GetProperLayout(image.width, image.height, difficulty);
        }

        public LayoutContract GetProperLayout(Texture2D image, int count)
        {
            return GetProperLayout(image.width, image.height, count);
        }

        public LayoutContract GetProperLayout(int width, int height, Difficulty difficulty)
        {
            var puzzleCount = difficulty.ToProperPuzzleCount();
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

        public SliceContract GetSlice(Texture2D image, LayoutContract layoutContract, SlicePattern slicePattern)
        {
            var sliceStrategy = _sliceStrategyFactory.Create(slicePattern);
            return sliceStrategy.GetSlice(image, layoutContract);
        }
    }
}
