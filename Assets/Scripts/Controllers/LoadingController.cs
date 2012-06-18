﻿using System.Collections;
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
        _backgroundImage = Resources.Load("Image/LevelBackground/3") as Texture2D;

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
        var surfaceThickness = GlobalConfiguration.SurfaceThicknessInMeter;
        var pieces = new List<GameObject>();

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                var pieceContract = pieceContracts[i, j];
                var name = _pieceService.GeneratePieceName(i, j);

                var piece = _pieceService.GeneratePiece(name, GetRandomPosition(0.2f), GetRandomRotation(), pieceContract.MappingMesh, pieceContract.BackseatMesh, new Color32(0xcc, 0xcc, 0xcc, 0xff), mainTexture, normalMap);

                GameObject.DontDestroyOnLoad(piece);
                pieces.Add(piece);

                _progress = (i * columns + j + 1) / pieceCount;

                yield return 1;
            }
        }

        foreach (var piece in pieces)
        {
            var boxCollider = piece.AddComponent<BoxCollider>();
            boxCollider.size += new Vector3(surfaceThickness, surfaceThickness, surfaceThickness);

            var rigidbody = piece.AddComponent<Rigidbody>();
            rigidbody.mass = 1f;
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
        return new Vector3(Random.Range(-range, range), Random.Range(-range, range) + GlobalConfiguration.DeskThinknessInMeter, Random.Range(-range, range));
    }

    Quaternion GetRandomRotation()
    {
        return Quaternion.Euler(new Vector3(Random.value > 0.5 ? 270 : 90, Random.Range(0f, 360f), 0));
    }

    bool IsPiecesStopedMoving()
    {
        return true;
    }
}
