using Tridimensional.Puzzle.Foundation;
using UnityEngine;

public class CrossingController : MonoBehaviour
{
    void Awake()
    {
        InitializationLight();
        InitializationCamera();
        InitializationEnvironment();
    }

    private void InitializationLight()
    {

    }

    private void InitializationCamera()
    {
        camera.backgroundColor = GlobalConfiguration.BackgroundColor;
    }

    private void InitializationEnvironment()
    {

    }
}