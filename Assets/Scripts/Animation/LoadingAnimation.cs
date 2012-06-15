using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    public float Progress;

    Rect _rect;
    Texture2D _background;
    Texture2D _foregroundLeft;
    Texture2D _foregroundMiddle;
    Texture2D _foregroundRight;

    public bool Finished { get { return Progress >= 1; } }

    void Awake()
    {
        _background = Resources.Load("Image/Logo/256") as Texture2D;
        _foregroundLeft = Resources.Load("Image/Loading/ForegroundLeft") as Texture2D;
        _foregroundMiddle = Resources.Load("Image/Loading/ForegroundMiddle") as Texture2D;
        _foregroundRight = Resources.Load("Image/Loading/ForegroundRight") as Texture2D;

        _rect = new Rect(100, 100, 100, 100);
    }

    void OnGUI()
    {
        GUI.DrawTexture(_rect, _background);
        GUI.DrawTexture(new Rect(100, 100, 100 * Progress, 100), _background);
    }
}
