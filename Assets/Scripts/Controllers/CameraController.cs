using UnityEngine;
using Tridimensional.Puzzle.Foundation;

public class CameraController : MonoBehaviour
{
    void Start()
    {
        gameObject.camera.nearClipPlane = 0.1f;
        gameObject.camera.backgroundColor = GlobalConfiguration.BackgroundColor;
        gameObject.camera.transform.position = new Vector3(0, 0, -(GlobalConfiguration.VisionHeightInMeter / 2) / Mathf.Tan(camera.fieldOfView / 2 * Mathf.PI / 180));
    }
}
