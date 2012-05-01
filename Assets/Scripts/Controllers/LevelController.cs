using System;
using Tridimensional.Puzzle.Foundation.Enumeration;
using Tridimensional.Puzzle.Service.Contract;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation;
using Tridimensional.Puzzle.Service.ServiceImplementation.SliceStrategy;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    IModelService _modelService;
    PieceContract[,] _pieceContracts;

    void Awake()
    {
        _modelService = new ModelService(new PieceService(), new SliceStrategyFactory());

        GeneratePuzzleModels();
    }

    void Update()
    {
        var pieces = GameObject.FindGameObjectsWithTag("Piece");

        foreach (var piece in pieces)
        {
            var row = Convert.ToInt32(piece.name.Substring(7, 3));
            var column = Convert.ToInt32(piece.name.Substring(11, 3));
            var distance = Vector3.Distance(piece.transform.position, _pieceContracts[row, column].Position);

            if (piece.transform.position.x - _pieceContracts[row, column].Position.x < 0.01)
            {
                piece.transform.position = _pieceContracts[row, column].Position;
            }
            else
            {
                piece.transform.position -= new Vector3(0.3f * Time.deltaTime, 0, 0);
            }
        }
    }

    void GeneratePuzzleModels()
    {
        var backgroundImage = Resources.Load("Image/LevelBackground/0") as Texture2D;
        var layoutContract = _modelService.GetProperLayout(Screen.width, Screen.height, 100);

        var sliceContract = _modelService.GetSlice(layoutContract, SliceProgram.Random);
        var pieceContracts = _modelService.GeneratePiece(sliceContract, backgroundImage);

        for (var i = 0; i < pieceContracts.GetLength(0); i++)
        {
            for (var j = 0; j < pieceContracts.GetLength(1); j++)
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
                go.transform.position += new Vector3(3 * (layoutContract.Width / 2f - go.transform.position.x) + 0.5f + UnityEngine.Random.value * 0.2f, 0, 0);
            }
        }

        _pieceContracts = pieceContracts;
    }
}
