using Tridimensional.Puzzle.Core;
using Tridimensional.Puzzle.Core.Enumeration;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation;
using UnityEngine;

public class CrossingController : MonoBehaviour
{
    ICrossingService _crossingService;
    ISceneService _sceneService;

    void Awake()
    {
        _crossingService = CrossingService.Instance;
        _sceneService = SceneService.Instance;

        _sceneService.Initialize(gameObject.camera);

        InitializeEnvironment();
    }

    private void InitializeEnvironment()
    {
        var gameContract = GameCommander.OpenNew();

        gameContract.ImageSource = "local";
        gameContract.OnlineType = OnlineType.Local;
        gameContract.SlicePattern = SlicePattern.Default;
        gameContract.Difficulty = Difficulty.Middle;
    }

    void Update()
    {
        Application.LoadLevel(LevelName.Loading.ToString());
    }
}
