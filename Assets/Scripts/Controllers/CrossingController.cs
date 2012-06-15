using Tridimensional.Puzzle.Core;
using Tridimensional.Puzzle.Core.Enumeration;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation;
using UnityEngine;

public class CrossingController : MonoBehaviour
{
    ICrossingService _crossingService;
    IPieceService _pieceService;
    IPuzzleService _puzzleService;
    ISceneService _sceneService;

    void Awake()
    {
        _crossingService = CrossingService.Instance;
        _pieceService = PieceService.Instance;
        _puzzleService = PuzzleService.Instance;
        _sceneService = SceneService.Instance;

        _sceneService.Initialize(camera);

        InitializeEnvironment();
    }

    void InitializeEnvironment()
    {
        var gameContract = GameCommander.OpenNew();
        var difficulty = Difficulty.Hard;
        var slicePattern = SlicePattern.Random;
        var image = Resources.Load("Image/LevelBackground/1") as Texture2D;
        var layoutContract = _puzzleService.GetProperLayout(image, difficulty);

        gameContract.ImageSource = "local";
        gameContract.OnlineType = OnlineType.Local;
        gameContract.SlicePattern = slicePattern;
        gameContract.Difficulty = difficulty;
        gameContract.Image = image;
        gameContract.SliceContract = _puzzleService.GetSlice(image, layoutContract, slicePattern);
    }

    void Update()
    {
        _pieceService.DestoryAllPieces();
        Application.LoadLevel(LevelName.Loading.ToString());
    }
}
