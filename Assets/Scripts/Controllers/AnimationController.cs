using Tridimensional.Puzzle.Core;
using Tridimensional.Puzzle.Core.Enumeration;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation;
using UnityEngine;
using System.Threading;

public class AnimationController : MonoBehaviour
{
    IPuzzleService _puzzleService;
    ISceneService _sceneService;

    void Awake()
    {
        _puzzleService = PuzzleService.Instance;
        _sceneService = SceneService.Instance;

        _sceneService.Initialize(gameObject.camera);

        InitializeEnvironment();
    }

    void InitializeEnvironment()
    {
        var backdropImage = Resources.Load("Image/LevelBackground/0") as Texture2D;

        var layoutContract = _puzzleService.GetProperLayout(Screen.width, Screen.height, 100);
        var sliceContract = _puzzleService.GetSlice(backdropImage, layoutContract, SlicePattern.Default);
        var pieceContracts = _puzzleService.GeneratePieceContracts(sliceContract);
        var backdropNormalMap = _puzzleService.GenerateNormalMap(sliceContract);

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
                var pieceName = GeneratePieceName(i, j);
                var distance = circleDistance + 4f * (visionWidth - 2 * (pieceContract.Position.x + pieceWidth * (UnityEngine.Random.value - 1)));
                var currentPosition = pieceContract.Position + new Vector3(distance - circleDistance, 0, 0);

                var piece = GeneratePiece(pieceName, currentPosition, pieceContract.MappingMesh, pieceContract.BackseatMesh, new Color32(0xcc, 0xcc, 0xcc, 0xff), backdropImage, backdropNormalMap);
                piece.tag = CustomTags.BackdropPiece.ToString();
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
        var pieces = GameObject.FindGameObjectsWithTag(CustomTags.BackdropPiece.ToString());

        foreach (var piece in pieces)
        {
            var openingAnimation = piece.GetComponent<OpeningAnimation>();
            if (!openingAnimation.Finished) { return; }
        }

        //Application.LoadLevel(LevelName.Crossing.ToString());
    }

    GameObject GeneratePiece(string name, Vector3  position, Mesh mappingMesh, Mesh backseatMesh, Color color, Texture2D mainTexture, Texture2D normalMap)
    {
        var go = new GameObject(name);
        go.AddComponent<MeshFilter>().mesh = backseatMesh;
        go.AddComponent<MeshRenderer>().material.color = color;
        go.transform.position = position;

        var mapping = new GameObject("Mapping");
        mapping.AddComponent<MeshFilter>().mesh = mappingMesh;
        mapping.AddComponent<MeshRenderer>().material = Resources.Load("Material/BumpedDiffuse") as Material;
        mapping.transform.renderer.material.SetTexture("_MainTex", mainTexture);
        mapping.transform.renderer.material.SetTexture("_BumpMap", normalMap);

        mapping.transform.parent = go.transform;
        mapping.transform.localPosition = new Vector3(0, 0, 0);

        return go;
    }

    string GeneratePieceName(int row, int column)
    {
        return string.Format("Piece <{0:000},{1:000}>", row, column);
    }
}
