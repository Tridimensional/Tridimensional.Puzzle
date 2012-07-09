using System;
using Tridimensional.Puzzle.Core;
using Tridimensional.Puzzle.Core.Enumeration;
using Tridimensional.Puzzle.Service.IServiceProvider;
using UnityEngine;

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

            if (levelName == LevelName.Battle)
            {
                camera.transform.position = GlobalConfiguration.DesktopPosition + new Vector3(0.0f, 1.0f, -0.0f);
                camera.transform.LookAt(GlobalConfiguration.DesktopPosition + new Vector3(0, GlobalConfiguration.DesktopThinkness / 2, 0));
            }
            else
            {
                camera.backgroundColor = GlobalConfiguration.BackgroundColor;
                camera.transform.position = new Vector3(0, 0, -GlobalConfiguration.CameraToSubject);
                camera.fieldOfView = 2 * Mathf.Atan(GlobalConfiguration.PictureHeight * 0.5f / GlobalConfiguration.CameraToSubject) * 180 / Mathf.PI;
            }

            if (levelName == LevelName.Loading)
            {
                GenerateDesktop();
            }
        }

        private void GenerateDesktop()
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "Desktop";
            go.transform.position = GlobalConfiguration.DesktopPosition;
            go.transform.localScale = new Vector3(100f, GlobalConfiguration.DesktopThinkness, 100f);
            go.transform.renderer.material.color = new Color32(0xff, 0xff, 0xff, 0xff);

            var boxCollider = go.GetComponent<BoxCollider>();

            GameObject.DontDestroyOnLoad(go);
        }
    }
}
