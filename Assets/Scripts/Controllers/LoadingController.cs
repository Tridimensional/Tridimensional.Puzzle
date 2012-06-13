using Tridimensional.Puzzle.Core;
using Tridimensional.Puzzle.Service.Contract;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation;
using UnityEngine;

public class LoadingController : MonoBehaviour
{
    GameContract _gameContract;
    ISceneService _sceneService;

    void Awake()
    {
        _sceneService = SceneService.Instance;

        _sceneService.Initialize(gameObject.camera);

        InitializeEnvironment();
    }

    private void InitializeEnvironment()
    {
        _gameContract = GameCommander.GameInstance;
    }

    void Update()
    {
 
    }
}