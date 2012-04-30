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
    private Texture2D MaskTexture;
    private IModelService _modelService;

    void Awake()
    {
        _modelService = new ModelService(new MeshService(), new SliceStrategyFactory());

        GeneratePuzzleModels();

        if (NeedSlideShow)
        {
            MaskTexture = new Texture2D(1, 1);
            MaskTexture.SetPixels(0, 0, 1, 1, new[] { GlobalConfiguration.BackgroundColor });
            MaskTexture.Apply();
        }
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
        var layoutContract = _modelService.GetProperLayout(Screen.width, Screen.height, 100);

        var sliceContract = _modelService.GetSlice(layoutContract, SliceProgram.Random);
        var meshes = _modelService.GenerateMesh(sliceContract, backgroundImage);

        for (var i = 0; i < meshes.GetLength(0); i++)
        {
            for (var j = 0; j < meshes.GetLength(1); j++)
            {
                var go = new GameObject(string.Format("Model ({0:00}, {1:00})", i, j));
                go.AddComponent<MeshFilter>().mesh = meshes[i, j].BackseatMesh;
                go.AddComponent<MeshRenderer>().material.color = Color.white;

                var mapping = new GameObject("Mapping");
                mapping.AddComponent<MeshFilter>().mesh = meshes[i, j].MappingMesh;
                mapping.AddComponent<MeshRenderer>().material.mainTexture = backgroundImage;
                mapping.transform.parent = go.transform;
            }
        }
    }

    Color GetCurrentColor(float duration, float offset)
    {
        var backgroundColor = GlobalConfiguration.BackgroundColor;

        return new Color
        {
            r = backgroundColor.r,
            g = backgroundColor.g,
            b = backgroundColor.b,
            a = GetCurrentAlpha(duration, offset, 1, 0)
        };
    }

    float GetCurrentAlpha(float duration, float offset, float start, float end)
    {
        return start + (end - start) * (float)Math.Pow(offset / duration, 3);
    }
}
