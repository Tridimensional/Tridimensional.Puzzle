using Tridimensional.Puzzle.Core;
using Tridimensional.Puzzle.Core.Entity;
using Tridimensional.Puzzle.Core.Enumeration;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    IGraphicsService _graphicsService;
    IPieceService _pieceService;
    IPuzzleService _puzzleService;
    ISceneService _sceneService;

    void Awake()
    {
        _graphicsService = GraphicsService.Instance;
        _pieceService = PieceService.Instance;
        _puzzleService = PuzzleService.Instance;
        _sceneService = SceneService.Instance;

        _sceneService.Initialize(camera);

        InitializeEnvironment();
    }

    void InitializeEnvironment()
    {
        var backdropImage = Resources.Load("Image/LevelBackground/0") as Texture2D;

        var layoutContract = _puzzleService.GetProperLayout(Screen.width, Screen.height, 100);
        var sliceContract = _puzzleService.GetSlice(backdropImage, layoutContract, SlicePattern.Default);
        var pieceContracts = _pieceService.GeneratePieceContracts(sliceContract);
        var backdropNormalMap = _graphicsService.GenerateNormalMap(sliceContract);

        var visionWidth = GlobalConfiguration.PictureHeightInMeter * Screen.width / Screen.height;
        var pieceWidth = visionWidth / layoutContract.Columns;
        var flightHeight = pieceWidth;
        var circleDistance = Mathf.PI * flightHeight / 2;
        var straightSpeed = GlobalConfiguration.PictureHeightInMeter;
        var corneringSpeed = straightSpeed * 0.2f;
        var rotationSpeed = 45 / pieceWidth;

        for (var i = 0; i < layoutContract.Rows; i++)
        {
            for (var j = 0; j < layoutContract.Columns; j++)
            {
                var pieceContract = pieceContracts[i, j];
                var pieceName = _pieceService.GeneratePieceName(i, j);
                var distance = circleDistance + 4f * (visionWidth - 2 * (pieceContract.Position.x + pieceWidth * (UnityEngine.Random.value - 1)));
                var currentPosition = pieceContract.Position + new Vector3(distance - circleDistance, 0, 0);

                var piece = _pieceService.GeneratePiece(pieceName, currentPosition, pieceContract.MappingMesh, pieceContract.BackseatMesh, new Color32(0xcc, 0xcc, 0xcc, 0xff), backdropImage, backdropNormalMap);
                GameObject.DontDestroyOnLoad(piece);

                var openingAnimation = piece.AddComponent<OpeningAnimation>();
                openingAnimation.CircleDistance = circleDistance;
                openingAnimation.CorneringSpeed = corneringSpeed;
                openingAnimation.Distance = distance;
                openingAnimation.FlightHeight = flightHeight;
                openingAnimation.PieceWidth = pieceWidth;
                openingAnimation.RotationSpeed = rotationSpeed;
                openingAnimation.StraightSpeed = straightSpeed;
                openingAnimation.VisionWidth = visionWidth;
                openingAnimation.Position = pieceContract.Position;
            }
        }
    }

    void Update()
    {
        var pieces = GameObject.FindGameObjectsWithTag(CustomTags.Piece.ToString());

        foreach (var piece in pieces)
        {
            var openingAnimation = piece.GetComponent<OpeningAnimation>();
            if (!openingAnimation.Finished) { return; }
        }

        Application.LoadLevel(LevelName.Crossing.ToString());
    }
}
