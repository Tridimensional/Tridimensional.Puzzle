using Tridimensional.Puzzle.Core;
using Tridimensional.Puzzle.Service.IServiceProvider;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation
{
    public class SceneService : MonoBehaviour, ISceneService
	{
        #region Instance

        static ISceneService _instance;

        public static ISceneService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SceneService();
                }
                return _instance;
            }
        }

        #endregion

        public void InitializationLight()
        {
            var go = new GameObject("Light");
            var light = go.AddComponent<Light>();

            light.intensity = 0.5f;
            light.type = LightType.Directional;
            light.transform.position = new Vector3(0, 0, -1);
            light.transform.rotation = Quaternion.Euler(30, 30, 0);
        }

        public void InitializationCamera(Camera camera)
        {
            camera.backgroundColor = GlobalConfiguration.BackgroundColor;
            camera.transform.position = new Vector3(0, 0, -GlobalConfiguration.CameraToSubjectInMeter);
            camera.fieldOfView = 2 * Mathf.Atan(GlobalConfiguration.PictureHeightInMeter * 0.5f / GlobalConfiguration.CameraToSubjectInMeter) * 180 / Mathf.PI;
        }
    }
}
