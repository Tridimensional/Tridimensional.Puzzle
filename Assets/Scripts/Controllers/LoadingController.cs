using System.Threading;
using Tridimensional.Puzzle.Core;
using Tridimensional.Puzzle.Service.Contract;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation;
using UnityEngine;

public class LoadingController : MonoBehaviour
{
    float _progress;
    int _totalSteps;
    int _completedSteps;
    GameContract _gameContract;
    IPuzzleService _puzzleService;
    ISceneService _sceneService;

    void Awake()
    {
        _puzzleService = PuzzleService.Instance;
        _sceneService = SceneService.Instance;

        _sceneService.Initialize(gameObject.camera);

        InitializeEnvironment();
    }

    void InitializeEnvironment()
    {
        _gameContract = GameCommander.GameInstance;

        var sliceContract = _gameContract.SliceContract;

        _totalSteps = sliceContract.Vertexes.GetLength(0) * sliceContract.Vertexes.GetLength(1);

        _puzzleService.GeneratePieceContracts(sliceContract, OnGui);
    }

    void OnGui()
    {
        _completedSteps++;
        _progress = 1.0f * _completedSteps / _totalSteps;

        var pos = new Vector2(100, 100);
        var size = new Vector2(200, 10);
        var emptyImage = Resources.Load("Image/Logo/48") as Texture2D;
        var progressImage = Resources.Load("Image/Logo/32") as Texture2D;

        GUI.DrawTexture(new Rect(pos.x, pos.y, size.x, size.y), emptyImage);
        GUI.DrawTexture(new Rect(pos.x, pos.y, size.x * Mathf.Clamp01(_progress), size.y), progressImage);
    }
}