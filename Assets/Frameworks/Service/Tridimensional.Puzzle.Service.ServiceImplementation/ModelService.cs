using System;
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
        IGraphicsService _graphicsService;
        SliceStrategyFactory _sliceStrategyFactory;

        public ModelService(IPieceService pieceService, IGraphicsService graphicsService, SliceStrategyFactory sliceStrategyFactory)
        {
            _pieceService = pieceService;
            _graphicsService = graphicsService;
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

        public SliceContract GetSlice(Texture2D image, LayoutContract layoutContract, SliceProgram sliceProgram)
        {
            var sliceStrategy = _sliceStrategyFactory.Create(sliceProgram);
            return sliceStrategy.GetSlice(image, layoutContract);
        }

        public GameObject GeneratePiece(string name, Vector3 position, Mesh mesh, Color color, Texture2D mainTexture, Texture2D normalMap)
        {
            var go = new GameObject(name);

            go.AddComponent<MeshFilter>().mesh = mesh;
            go.AddComponent<MeshRenderer>().material = Resources.Load("Material/BumpedDiffuse") as Material;
            go.transform.renderer.material.color = color;
            go.transform.renderer.material.SetTexture("_MainTex", mainTexture);
            go.transform.renderer.material.SetTexture("_BumpMap", normalMap);
            go.transform.position = position;

            return go;
        }

        public string GeneratePieceName(int row, int column)
        {
            return string.Format("Piece <{0:000},{1:000}>", row, column);
        }

        public PieceContract[,] GeneratePieceContracts(SliceContract sliceContract)
        {
            return _pieceService.GeneratePiece(sliceContract);
        }

        public Texture2D GenerateNormalMap(SliceContract sliceContract)
        {
            var heightMap = _graphicsService.GenerateHeightMap(sliceContract);
            return _graphicsService.GenerateNormalMap(heightMap);
        }
    }
}
