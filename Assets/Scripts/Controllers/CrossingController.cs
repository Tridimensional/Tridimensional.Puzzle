using Tridimensional.Puzzle.Core.Enumeration;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation;
using UnityEngine;

public class CrossingController : MonoBehaviour
{
    ICrossingService _crossingService;
    IGameService _gameService;
    IPieceService _pieceService;
    IPuzzleService _puzzleService;
    ISceneService _sceneService;

    void Awake()
    {
        _crossingService = CrossingService.Instance;
        _gameService = GameService.Instance;
        _pieceService = PieceService.Instance;
        _puzzleService = PuzzleService.Instance;
        _sceneService = SceneService.Instance;

        _sceneService.Initialize(camera);

        InitializeEnvironment();
    }

    void InitializeEnvironment()
    {
        var gameContract = _gameService.OpenNew();

        gameContract.ImageSource = ImageSource.Local;
        gameContract.OnlineType = OnlineType.Local;
        gameContract.SlicePattern = SlicePattern.Default;
        gameContract.Difficulty = Difficulty.Middle;
        gameContract.ImageAddress = "Image/LevelBackground/4";

        _gameService.Apply();
    }

    void Update()
    {
        _pieceService.DestoryAllPieces();
        Application.LoadLevel(LevelName.Loading.ToString());
    }
}
