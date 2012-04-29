using System;
using Tridimensional.Puzzle.Foundation;
using Tridimensional.Puzzle.Foundation.Enumeration;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation;
using Tridimensional.Puzzle.Service.ServiceImplementation.SliceStrategy;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public bool NeedSlideShow = false;
    private float Duration = 2;
    private Color BackgroundColor;
    private Texture2D MaskTexture;
    private IModelService _modelService;

    void Awake()
    {
        _modelService = new ModelService(new MeshService(), new SliceStrategyFactory());

        GeneratePuzzleModels();

        BackgroundColor = GlobalConfiguration.BackgroundColor;

        if (NeedSlideShow)
        {
            MaskTexture = new Texture2D(1, 1);
            MaskTexture.SetPixels(0, 0, 1, 1, new[] { BackgroundColor });
            MaskTexture.Apply();
        }
    }

    void Start()
    {
        camera.backgroundColor = Color.black;
    }

    void OnGUI()
    {
        if (NeedSlideShow && Time.timeSinceLevelLoad <= Duration)
        {
            GUI.color = GetCurrentColor(Duration, Time.timeSinceLevelLoad);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), MaskTexture);
        }
    }

    void GeneratePuzzleModels()
    {
        var backgroundImage = Resources.Load("Image/LevelBackground/0") as Texture2D;
        var formation = _modelService.GetProperFormation(backgroundImage.width, backgroundImage.height, 100);

        var sliceContract = _modelService.GetSlice(formation, SliceProgram.Random);
        var meshes = _modelService.GenerateMesh(sliceContract);

        for (var i = 0; i < meshes.GetLength(0); i++)
        {
            for (var j = 0; j < meshes.GetLength(1); j++)
            {
                var go = new GameObject(string.Format("Model ({0}, {1})", i, j));

                var meshFilter = go.AddComponent<MeshFilter>();
                meshFilter.mesh = meshes[i, j];

                var meshRender = go.AddComponent<MeshRenderer>();
                meshRender.material.color = Color.white;
				meshRender.material.mainTexture = backgroundImage;
            }
        }
    }

    Color GetCurrentColor(float duration, float offset)
    {
        return new Color
        {
            r = BackgroundColor.r,
            g = BackgroundColor.g,
            b = BackgroundColor.b,
            a = GetCurrentValue(duration, offset, 1, 0)
        };
    }

    float GetCurrentValue(float duration, float offset, float start, float end)
    {
        return start + (end - start) * (float)Math.Pow(offset / duration, 3);
    }
}
