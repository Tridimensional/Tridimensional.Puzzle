using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    public float Progress;

    int width;
    int height;
    Texture2D _background;
    Texture2D _foregroundLeft;
    Texture2D _foregroundMiddle;
    Texture2D _foregroundRight;

    public bool Finished { get { return Progress >= 1; } }

    void Awake()
    {
        _background = Resources.Load("Image/LevelBackground/4") as Texture2D;
        _foregroundLeft = Resources.Load("Image/LevelBackground/1") as Texture2D;
        _foregroundMiddle = Resources.Load("Image/LevelBackground/1") as Texture2D;
        _foregroundRight = Resources.Load("Image/LevelBackground/1") as Texture2D;

        width = Screen.width / 2;
        height = width / 30;
    }

    void OnGUI()
    {
        var backgroundRect = new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height);
        var foregroundLeftRect = new Rect(backgroundRect.xMin, backgroundRect.yMin, height / 2, height);
        var foregroundMiddleRect = new Rect(foregroundLeftRect.xMax, backgroundRect.yMin, Progress * (width - height), height);
        var foregroundRightRect = new Rect(foregroundMiddleRect.xMax, backgroundRect.yMin, height / 2, height);

        GUI.DrawTexture(backgroundRect, _background);
        GUI.DrawTexture(foregroundLeftRect, _foregroundLeft);
        GUI.DrawTexture(foregroundMiddleRect, _foregroundMiddle);
        GUI.DrawTexture(foregroundRightRect, _foregroundRight);
    }
}
