using System;
using Tridimensional.Puzzle.Foundation.Enumeration;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation;
using Tridimensional.Puzzle.Service.ServiceImplementation.SliceStrategy;
using Tridimensional.Puzzle.UI.ViewModel;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    IModelService _modelService;
    PieceViewModel[,] _pieceViewModels;
    float _flightHeight;
    float _flightExcess;
    float _rotationCoefficient;

    void Awake()
    {
        _modelService = new ModelService(new PieceService(), new SliceStrategyFactory());

        GeneratePuzzleModels();
    }

    void Update()
    {
        var rows = _pieceViewModels.GetLength(0);
        var columns = _pieceViewModels.GetLength(1);

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                Animation(_pieceViewModels[i, j]);
            }
        }
    }

    private void Animation(PieceViewModel pieceViewModel)
    {
        var piece = pieceViewModel.Object;
        var target = pieceViewModel.Position;
        var current = piece.transform.position;

        if (!pieceViewModel.MovingCompleted)
        {
            var offset = 0.4f * Time.deltaTime;

            if (current.x - target.x < offset - _flightExcess)
            {
                piece.transform.position = new Vector3(target.x - _flightExcess, target.y, -_flightHeight);
                pieceViewModel.MovingCompleted = true;
            }
            else
            {
                piece.transform.position = new Vector3(current.x - offset, current.y, -_flightHeight);
            }

            piece.transform.rotation = Quaternion.Euler(0, -_rotationCoefficient * (piece.transform.position.x - target.x), 0);
        }
        else if (!pieceViewModel.LandingCompleted)
        {
            var offset = 0.05f * Time.deltaTime;

            if (target.x - current.x < offset)
            {
                piece.transform.position = target;
                pieceViewModel.LandingCompleted = true;
            }
            else
            {
                piece.transform.position = new Vector3(current.x + offset, current.y, current.z + _flightExcess / _flightHeight * offset);
            }

            piece.transform.rotation = Quaternion.Euler(0, _rotationCoefficient * (piece.transform.position.x - target.x), 0);
        }
    }

    void GeneratePuzzleModels()
    {
        var backgroundImage = Resources.Load("Image/LevelBackground/0") as Texture2D;
        var layoutContract = _modelService.GetProperLayout(Screen.width, Screen.height, 100);

        var sliceContract = _modelService.GetSlice(layoutContract, SliceProgram.Random);
        var pieceContracts = _modelService.GeneratePiece(sliceContract, backgroundImage);

        var rows = pieceContracts.GetLength(0);
        var columns = pieceContracts.GetLength(1);
        var unitRange = layoutContract.Height / layoutContract.Rows;

        _pieceViewModels = new PieceViewModel[rows, columns];
        _flightHeight = unitRange;
        _flightExcess = unitRange;
        _rotationCoefficient = 180 / _flightExcess;

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                var go = new GameObject(string.Format("Piece <{0:000},{1:000}>", i, j));
                go.AddComponent<MeshFilter>().mesh = pieceContracts[i, j].BackseatMesh;
                go.AddComponent<MeshRenderer>().material.color = Color.white;

                var mapping = new GameObject("Mapping");
                mapping.AddComponent<MeshFilter>().mesh = pieceContracts[i, j].MappingMesh;
                mapping.AddComponent<MeshRenderer>().material.mainTexture = backgroundImage;
                mapping.transform.parent = go.transform;

                go.tag = "Piece";
                go.transform.position = pieceContracts[i, j].Position;
                go.transform.position += new Vector3(8 * (layoutContract.Width / 2f - go.transform.position.x + UnityEngine.Random.value * unitRange), 0, 0);

                _pieceViewModels[i, j] = new PieceViewModel { Object = go, Position = pieceContracts[i, j].Position, MovingCompleted = false, LandingCompleted = false };
            }
        }
    }
}
