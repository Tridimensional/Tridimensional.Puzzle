using System;
using Tridimensional.Puzzle.Foundation;
using Tridimensional.Puzzle.Foundation.Enumeration;
using UnityEngine;

public class WelcomeController : MonoBehaviour
{
    private float Duration = 0.5f;
    private Texture2D LogoTexture;
    private Rect LogoTextureRect;
    private Color BackgroundColor;

    void Awake()
    {
        LogoTexture = Resources.Load("Logo/Logo") as Texture2D;
        LogoTextureRect = GetCompatibleLogoRect();
        BackgroundColor = GlobalConfiguration.BackgroundColor;
    }

    void Start()
    {
        camera.backgroundColor = BackgroundColor;
    }

    void OnGUI()
    {
        var stillness = Duration * 1 / 3;

        GUI.color = Time.time < stillness ? BackgroundColor : new Color(BackgroundColor.r, BackgroundColor.g, BackgroundColor.b, GetCurrentValue(Duration - stillness, Time.time - stillness, 1, 0));
        GUI.DrawTexture(LogoTextureRect, LogoTexture);

        if (Time.time > Duration)
        {
            Application.LoadLevel(LevelName.ModeSelection.ToString());
        }
    }

    float GetCurrentValue(float duration, float offset, float start, float end)
    {
        return start + (end - start) * (float)Math.Pow(offset / duration, 3);
    }

    Rect GetCompatibleLogoRect()
    {
        return new Rect((Screen.width - LogoTexture.width) / 2, (Screen.height - LogoTexture.height) / 2, LogoTexture.width, LogoTexture.height);
    }
}
