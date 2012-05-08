using System;
using Tridimensional.Puzzle.Foundation;
using Tridimensional.Puzzle.Foundation.Enumeration;
using UnityEngine;

public class WelcomeController : MonoBehaviour
{
    float _duration = 3f;
    Texture2D _logoTexture;
    Rect _logoTextureRect;
    Color _backgroundColor;

    void Awake()
    {
        _logoTexture = Resources.Load("Logo/Logo") as Texture2D;
        _logoTextureRect = GetCompatibleLogoRect();
        _backgroundColor = GlobalConfiguration.BackgroundColor;
    }

    void Start()
    {
        camera.backgroundColor = _backgroundColor;
    }

    void OnGUI()
    {
        var stillness = _duration * 1 / 3;

        GUI.color = Time.time < stillness ? _backgroundColor : new Color(_backgroundColor.r, _backgroundColor.g, _backgroundColor.b, GetCurrentValue(_duration - stillness, Time.time - stillness, 1, 0));
        GUI.DrawTexture(_logoTextureRect, _logoTexture);

        if (Time.time > _duration)
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
        return new Rect((Screen.width - _logoTexture.width) / 2, (Screen.height - _logoTexture.height) / 2, _logoTexture.width, _logoTexture.height);
    }
}
