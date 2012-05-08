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
        IPieceService _pieceService;
        SliceStrategyFactory _sliceStrategyFactory;

        public ModelService(IPieceService pieceService, SliceStrategyFactory sliceStrategyFactory)
        {
            _pieceService = pieceService;
            _sliceStrategyFactory = sliceStrategyFactory;
        }

        public LayoutContract GetProperLayout(Texture2D image, GameDifficulty gameDifficulty)
        {
            var puzzleCount = gameDifficulty.ToProperPuzzleCount();
            return GetProperLayout(image, puzzleCount);
        }

        public LayoutContract GetProperLayout(Texture2D image, int count)
        {
            var width = image.width > image.height ? GlobalConfiguration.PictureScaleInMeter * image.width / image.height : GlobalConfiguration.PictureScaleInMeter;
            var height = image.width > image.height ? GlobalConfiguration.PictureScaleInMeter : GlobalConfiguration.PictureScaleInMeter * image.height / image.width;

            return GetProperLayout(width, height, count);
        }

        public LayoutContract GetProperLayout(float width, float height, GameDifficulty gameDifficulty)
        {
            var puzzleCount = gameDifficulty.ToProperPuzzleCount();
            return GetProperLayout(width, height, puzzleCount);
        }

        public LayoutContract GetProperLayout(float width, float height, int count)
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
            var sliceWidth = sliceContract.Vertexes[0, sliceContract.Vertexes.GetLength(1) - 1].x - sliceContract.Vertexes[0, 0].x;
            var sliceHeight = sliceContract.Vertexes[sliceContract.Vertexes.GetLength(0) - 1, 0].y - sliceContract.Vertexes[0, 0].y;

            var mappingOffset = new Vector2(0, 0);

            if (sliceWidth * image.height > sliceHeight * image.width) { mappingOffset.y = (sliceWidth * image.height / image.width - sliceHeight) / 2; }
            else { mappingOffset.x = (sliceHeight * image.width / image.height - sliceWidth) / 2; }

            return _pieceService.GeneratePiece(sliceContract, mappingOffset);
        }
    }
}
