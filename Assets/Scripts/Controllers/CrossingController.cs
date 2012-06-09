using Tridimensional.Puzzle.Foundation;
using Tridimensional.Puzzle.IOC;
using Tridimensional.Puzzle.Service.IServiceProvider;
using UnityEngine;

public class CrossingController : MonoBehaviour
{
    ICrossingService _crossingService;

    void Awake()
    {
        InitializationEnvironment();
        InitializationCamera();
        InitializationLight();
    }

    private void InitializationLight()
    {
        var go = new GameObject("Light");
        var light = go.AddComponent<Light>();

        light.intensity = 0.5f;
        light.type = LightType.Directional;
        light.transform.position = new Vector3(0, 0, -1);
        light.transform.rotation = Quaternion.Euler(30, 30, 0);
    }

    private void InitializationCamera()
    {
        camera.backgroundColor = GlobalConfiguration.BackgroundColor;
        camera.transform.position = new Vector3(0, 0, -GlobalConfiguration.CameraToSubjectInMeter);
        camera.fieldOfView = 2 * Mathf.Atan(GlobalConfiguration.PictureHeightInMeter * 0.5f / GlobalConfiguration.CameraToSubjectInMeter) * 180 / Mathf.PI;
    }

    private void InitializationEnvironment()
    {
        _crossingService = InjectionRepository.Instance.Get<ICrossingService>();
    }
}
