using System;
using Tridimensional.Puzzle.Core.Enumeration;
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

        #region Instance

        static IModelService _instance;

        public static IModelService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ModelService();
                }
                return _instance;
            }
        }

        #endregion

        public ModelService()
        {
            _pieceService = PieceService.Instance;
            _graphicsService = GraphicsService.Instance;
            _sliceStrategyFactory = new SliceStrategyFactory();
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

        public GameObject GeneratePiece(string name, Vector3 position, Mesh mappingMesh, Mesh backseatMesh, Color color, Texture2D mainTexture, Texture2D normalMap)
        {
            var go = new GameObject(name);
            go.AddComponent<MeshFilter>().mesh = backseatMesh;
            go.AddComponent<MeshRenderer>().material.color = color;
            go.transform.position = position;

            var mapping = new GameObject("Mapping");
            mapping.AddComponent<MeshFilter>().mesh = mappingMesh;
            mapping.AddComponent<MeshRenderer>().material = Resources.Load("Material/BumpedDiffuse") as Material;
            mapping.transform.renderer.material.SetTexture("_MainTex", mainTexture);
            mapping.transform.renderer.material.SetTexture("_BumpMap", normalMap);

            mapping.transform.parent = go.transform;
            mapping.transform.localPosition = new Vector3(0, 0, 0);

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
