using System;
using System.Collections;
using System.Collections.Generic;
using Tridimensional.Puzzle.Core;
using Tridimensional.Puzzle.Core.Entity;
using Tridimensional.Puzzle.Core.Enumeration;
using Tridimensional.Puzzle.Service.Contract;
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

        StartCoroutine(AsyncLoadObjects());
    }

    IEnumerator AsyncLoadObjects()
    {
        yield return 0;

        var mainTexture = _gameService.GetMainTexture();
        var pieceContracts = _gameService.GetPieceContracts();
        var normalMap = _gameService.GetNormalMap();

        var rows = pieceContracts.GetLength(0);
        var columns = pieceContracts.GetLength(1);
        var pieceCount = (float)rows * columns;
        var pieces = new List<GameObject>();

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                var pieceContract = pieceContracts[i, j];
                var name = _pieceService.GeneratePieceName(i, j);

                var piece = _pieceService.GeneratePiece(name, Vector3.zero, pieceContract.MappingMesh, pieceContract.BackseatMesh, new Color32(0xcc, 0xcc, 0xcc, 0xff), mainTexture, normalMap);
                var boxCollider = piece.AddComponent<BoxCollider>();
                var rigidbody = piece.AddComponent<Rigidbody>();

                GameObject.DontDestroyOnLoad(piece);
                pieces.Add(piece);

                _progress = (i * columns + j + 1) / pieceCount;

                yield return 1;
            }
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

    bool IsPiecesStopedMoving()
    {
        return true;
    }
}
