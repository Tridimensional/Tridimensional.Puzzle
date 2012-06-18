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

            if (levelName == LevelName.Battle)
            {
                camera.transform.position = GlobalConfiguration.DeskPosition + new Vector3(0.4f, 0.8f, 0.4f);
                camera.transform.LookAt(GlobalConfiguration.DeskPosition + new Vector3(0, GlobalConfiguration.DeskThinknessInMeter / 2, 0));
            }
            else
            {
                camera.backgroundColor = GlobalConfiguration.BackgroundColor;
                camera.transform.position = new Vector3(0, 0, -GlobalConfiguration.CameraToSubjectInMeter);
                camera.fieldOfView = 2 * Mathf.Atan(GlobalConfiguration.PictureHeightInMeter * 0.5f / GlobalConfiguration.CameraToSubjectInMeter) * 180 / Mathf.PI;
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
            go.transform.position = GlobalConfiguration.DeskPosition;
            go.transform.localScale = new Vector3(10f, GlobalConfiguration.DeskThinknessInMeter, 10f);
            go.transform.renderer.material.color = new Color32(0xff, 0x00, 0x00, 0xff);

            var boxCollider = go.GetComponent<BoxCollider>();
            boxCollider.size += new Vector3(0, GlobalConfiguration.SurfaceThicknessInMeter * 2, 0);

            GameObject.DontDestroyOnLoad(go);
        }
    }
}
