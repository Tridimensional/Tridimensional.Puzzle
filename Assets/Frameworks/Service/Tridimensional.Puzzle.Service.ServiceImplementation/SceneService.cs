using Tridimensional.Puzzle.Core;
using Tridimensional.Puzzle.Service.IServiceProvider;
using UnityEngine;
using Tridimensional.Puzzle.Core.Enumeration;
using System;

namespace Tridimensional.Puzzle.Service.ServiceImplementation
{
    public class SceneService : ISceneService
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

        public void Initialize(Camera camera)
        {
            var levelName = (LevelName)Enum.Parse(typeof(LevelName), Application.loadedLevelName);

            var light = new GameObject("Light").AddComponent<Light>();

            light.intensity = 0.5f;
            light.type = LightType.Directional;
            light.transform.position = new Vector3(0, 0, -1);
            light.transform.rotation = Quaternion.Euler(30, 30, 0);

            camera.backgroundColor = GlobalConfiguration.BackgroundColor;
            camera.transform.position = new Vector3(0, 0, -GlobalConfiguration.CameraToSubjectInMeter);
            camera.fieldOfView = 2 * Mathf.Atan(GlobalConfiguration.PictureHeightInMeter * 0.5f / GlobalConfiguration.CameraToSubjectInMeter) * 180 / Mathf.PI;

            if (levelName == LevelName.Loading)
            {
                GenerateDesktop();
            }
        }

        private void GenerateDesktop()
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "Desktop";
            go.transform.position = new Vector3(0, -0.2f, 0);
            go.transform.localScale = new Vector3(1f, 0.1f, 1f);
            go.transform.renderer.material.color = new Color32(0xcc, 0xcc, 0xcc, 0xff);

            var boxCollider = go.GetComponent<BoxCollider>();

            GameObject.DontDestroyOnLoad(go);
        }
    }
}
