using System.Threading;
using Tridimensional.Puzzle.Core;
using Tridimensional.Puzzle.Core.Entity;
using Tridimensional.Puzzle.Core.Enumeration;
using Tridimensional.Puzzle.Service.Contract;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation;
using UnityEngine;

public class LoadingController : MonoBehaviour
{
    float _progress;
    GameContract _gameContract;
    GameObject _loading;
    IGraphicsService _graphicsService;
    ILoadingService _loadingService;
    Image _normalMapImage;
    IPieceService _pieceService;
    IPuzzleService _puzzleService;
    ISceneService _sceneService;
    PieceContract[,] _pieceContracts;

    void Awake()
    {
        _graphicsService = GraphicsService.Instance;
        _loadingService = LoadingService.Instance;
        _pieceService = PieceService.Instance;
        _puzzleService = PuzzleService.Instance;
        _sceneService = SceneService.Instance;

        _sceneService.Initialize(camera);

        InitializeEnvironment();
    }

    void InitializeEnvironment()
    {
        _gameContract = GameCommander.GameInstance;
        _loading = GenerateLoading();

        var pieceProgress = 0f;
        var normalMapProgress = 0f;
        var sliceContract = _gameContract.SliceContract;

        new Thread(() => { _pieceContracts = _pieceService.GeneratePieceContracts(sliceContract, refer => { OnPercentageCompleted(pieceProgress = refer, normalMapProgress); }); }).Start();
        new Thread(() => { _normalMapImage = _graphicsService.GenerateNormalMap(sliceContract, 1, refer => { OnPercentageCompleted(pieceProgress, normalMapProgress = refer); }); }).Start();
    }

    void OnPercentageCompleted(float pieceProgress, float normalMapProgress)
    {
        _progress = 0.3f * pieceProgress + 0.7f * normalMapProgress;
    }

    void Update()
    {
        var loadingAnimation = _loading.GetComponent<LoadingAnimation>();
        loadingAnimation.Progress = _progress;

        if (_pieceContracts != null && _normalMapImage != null)
        {
            var pieces = GameObject.FindGameObjectsWithTag(CustomTags.Piece.ToString());

            if (pieces == null || pieces.Length == 0)
            {
                GeneratePiece(_pieceContracts, _gameContract.Image, _normalMapImage.ToTexture2D());
            }
            else if (IsPiecesStopMoving(pieces))
            {
                Application.LoadLevel(LevelName.Battle.ToString());
            }
        }

        if (_progress < 1) { Debug.Log(_progress); }
    }

    private bool IsPiecesStopMoving(GameObject[] gameObjects)
    {
        return true;
    }

    void GeneratePiece(PieceContract[,] pieceContracts, Texture2D mainTexture, Texture2D normalMap)
    {
        var rows = pieceContracts.GetLength(0);
        var columns = pieceContracts.GetLength(1);

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                var pieceContract = pieceContracts[i, j];
                var name = _pieceService.GeneratePieceName(i, j);
                var mappingMesh = _pieceService.ConvertToMappingMesh(pieceContract.MappingMesh);
                var backseatMesh = _pieceService.ConvertToBackseatMesh(pieceContract.BackseatMesh);

                var piece = _pieceService.GeneratePiece(name, pieceContract.Position, mappingMesh, backseatMesh, new Color32(0xcc, 0xcc, 0xcc, 0xff), mainTexture, normalMap);
                GameObject.DontDestroyOnLoad(piece);
            }
        }
    }

    GameObject GenerateLoading()
    {
        var go = new GameObject("Loading");

        var loading = go.AddComponent<LoadingAnimation>();

        return go;
    }
}
