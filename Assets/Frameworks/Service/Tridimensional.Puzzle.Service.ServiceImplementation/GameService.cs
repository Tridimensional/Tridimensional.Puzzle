using Tridimensional.Puzzle.Service.Contract;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation.ImageSourceStrategy;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation
{
	public class GameService : IGameService
	{
        #region Instance

        static IGameService _instance;

        public static IGameService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameService();
                }
                return _instance;
            }
        }

        #endregion

        #region Game Instances

        static GameContract _gameContract;
        static Texture2D _mainTexture;
        static Texture2D _normalMap;
        static PieceContract[,] _pieceContracts;

        #endregion

        IGraphicsService _graphicsService;
        ImageSourceStrategyFactory _imageSourceStrategyFactory;
        IPieceService _pieceService;
        IPuzzleService _puzzleService;
        
        public GameService()
        {
            _graphicsService = GraphicsService.Instance;
            _imageSourceStrategyFactory = new ImageSourceStrategyFactory();
            _pieceService = PieceService.Instance;
            _puzzleService = PuzzleService.Instance;
        }

        public GameContract CurrentGame
        {
            get { return _gameContract; }
        }

        public GameContract OpenNew()
        {
            _gameContract = new GameContract();
            return _gameContract;
        }

        public Texture2D GetMainTexture()
        {
            return _mainTexture;
        }

        public Texture2D GetNormalMap()
        {
            return _normalMap;
        }

        public PieceContract[,] GetPieceContracts()
        {
            return _pieceContracts;
        }

        public void Apply()
        {
            var imageSourceStrategy = _imageSourceStrategyFactory.Create(_gameContract.ImageSource);
            _mainTexture = imageSourceStrategy.GetImage(_gameContract);

            var layoutContract = _puzzleService.GetProperLayout(_mainTexture, _gameContract.Difficulty);
            var sliceContract = _puzzleService.GetSlice(_mainTexture, layoutContract, _gameContract.SlicePattern);

            _normalMap = _graphicsService.GenerateNormalMap(sliceContract);
            _pieceContracts = _pieceService.GeneratePieceContracts(sliceContract);
        }
    }
}
