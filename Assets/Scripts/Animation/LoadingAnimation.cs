using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    public float Progress;

    int _width;
    int _height;
    Texture2D _backgroundLeft;
    Texture2D _backgroundMiddle;
    Texture2D _backgroundRight;
    Texture2D _foregroundLeft;
    Texture2D _foregroundMiddle;
    Texture2D _foregroundRight;

    public bool Finished { get { return Progress >= 1; } }

    void Awake()
    {
        _backgroundLeft = Resources.Load("Image/Loading/BackgroundLeft") as Texture2D;
        _backgroundMiddle = Resources.Load("Image/Loading/BackgroundMiddle") as Texture2D;
        _backgroundRight = Resources.Load("Image/Loading/BackgroundRight") as Texture2D;
        _foregroundLeft = Resources.Load("Image/Loading/ForegroundLeft") as Texture2D;
        _foregroundMiddle = Resources.Load("Image/Loading/ForegroundMiddle") as Texture2D;
        _foregroundRight = Resources.Load("Image/Loading/ForegroundRight") as Texture2D;

        _width = Screen.width / 3;
        _height = _width / 10;
    }

    void OnGUI()
    {
        GUI.depth = 0;

        var backgroundLeftRect = new Rect((Screen.width - _width) / 2, (Screen.height - _height) / 2, _height / 2, _height);
        var backgroundMiddleRect = new Rect(backgroundLeftRect.xMax, backgroundLeftRect.yMin, _width - _height, _height);
        var backgroundRightRect = new Rect(backgroundMiddleRect.xMax, backgroundLeftRect.yMin, _height / 2, _height);

        GUI.DrawTexture(backgroundLeftRect, _backgroundLeft);
        GUI.DrawTexture(backgroundMiddleRect, _backgroundMiddle);
        GUI.DrawTexture(backgroundRightRect, _backgroundRight);

        if (Progress > 0)
        {
            var forgroundLeftRect = backgroundLeftRect;
            var forgroundMiddleRect = new Rect(forgroundLeftRect.xMax, forgroundLeftRect.yMin, Progress * (_width - _height), _height);
            var forgroundRightRect = new Rect(forgroundMiddleRect.xMax, forgroundLeftRect.yMin, _height / 2, _height);

            GUI.DrawTexture(forgroundLeftRect, _foregroundLeft);
            GUI.DrawTexture(forgroundMiddleRect, _foregroundMiddle);
            GUI.DrawTexture(forgroundRightRect, _foregroundRight);
        }
    }
}
