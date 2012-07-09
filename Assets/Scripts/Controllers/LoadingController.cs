using System.Collections;
using System.Collections.Generic;
using Tridimensional.Puzzle.Core;
using Tridimensional.Puzzle.Core.Enumeration;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation;
using UnityEngine;

public class LoadingController : MonoBehaviour
{
    float _progress = 0;
    IGameService _gameService;
    IGraphicsService _graphicsService;
    ILoadingService _loadingService;
    IPieceService _pieceService;
    IPuzzleService _puzzleService;
    ISceneService _sceneService;
    GameObject[] _pieces;
    LoadingAnimation _loadingAnimation;
    Texture2D _backgroundImage;

    void Awake()
    {
        _gameService = GameService.Instance;
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
        _loadingAnimation = gameObject.AddComponent<LoadingAnimation>();
        _backgroundImage = Resources.Load("Image/LevelBackground/2") as Texture2D;

        StartCoroutine(AsyncLoadObjects());
    }

    IEnumerator AsyncLoadObjects()
    {
        var mainTexture = null as Texture2D;
        var pieceContracts = _gameService.GetPieceContracts();
        var normalMap = null as Texture2D;

        var rows = pieceContracts.GetLength(0);
        var columns = pieceContracts.GetLength(1);
        var pieces = new List<GameObject>();

        var index = 0;
        var pieceCount = (float)rows * columns;
        var penetration = GlobalConfiguration.Penetration * 2;

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                var pieceContract = pieceContracts[i, j];
                var name = _pieceService.GeneratePieceName(i, j);

                var piece = _pieceService.GeneratePiece(name, GetRandomPosition(0.25f), GetRandomRotation(), pieceContract.MappingMesh, pieceContract.BackseatMesh, new Color32(0xcc, 0xcc, 0xcc, 0xff), mainTexture, normalMap);
                //piece.AddComponent<PieceBehaviour>();

                var boxCollider = piece.AddComponent<BoxCollider>();
                boxCollider.size += new Vector3(penetration, penetration, penetration);

                GameObject.DontDestroyOnLoad(piece);
                pieces.Add(piece);

                yield return 0;

                _progress = ++index / pieceCount;
            }
        }

        foreach (var piece in pieces)
        {
            var rigidbody = piece.AddComponent<Rigidbody>();
        }

        _pieces = pieces.ToArray();
    }

    void Update()
    {
        _loadingAnimation.Progress = _progress;

        if (_loadingAnimation.Finished && IsPiecesStopedMoving())
        {
            Application.LoadLevel(LevelName.Battle.ToString());
        }
    }

    void OnGUI()
    {
        GUI.depth = 1;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _backgroundImage);
    }

    Vector3 GetRandomPosition(float range)
    {
        var angle = Random.Range(0f, 360f);
        var distance = Random.Range(0f, range);

        return new Vector3(distance * Mathf.Cos(angle), Random.Range(0, range) + GlobalConfiguration.DesktopThinkness, distance * Mathf.Sin(angle));
    }

    Quaternion GetRandomRotation()
    {
        return Quaternion.Euler(new Vector3(Random.value > 0.5 ? 270 : 90, Random.Range(0f, 360f), 0));
    }

    bool IsPiecesStopedMoving()
    {
        var rigidbody = null as Rigidbody;

        //foreach (var piece in _pieces)
        //{
        //    rigidbody = piece.GetComponent<Rigidbody>();
        //    if (rigidbody == null || !rigidbody.IsSleeping()) { return false; }
        //}

        return true;
    }
}
