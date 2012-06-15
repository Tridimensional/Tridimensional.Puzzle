using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    ISceneService _sceneService;

    void Awake()
    {
        _sceneService = SceneService.Instance;

        _sceneService.Initialize(camera);
    }
}
