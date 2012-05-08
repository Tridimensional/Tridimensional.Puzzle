using UnityEngine;
using Tridimensional.Puzzle.Foundation;

public class CameraController : MonoBehaviour
{
    void Start()
    {
        gameObject.camera.backgroundColor = GlobalConfiguration.BackgroundColor;
        gameObject.camera.transform.position = new Vector3(0, 0, -GlobalConfiguration.CameraToSubjectInMeter);
        gameObject.camera.fieldOfView = 2 * Mathf.Atan(GlobalConfiguration.PictureScaleInMeter / 2 / GlobalConfiguration.CameraToSubjectInMeter) * 180 / Mathf.PI;
    }
}
