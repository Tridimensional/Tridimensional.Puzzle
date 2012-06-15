using System;
using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
	public interface IPieceService
	{
        PieceContract[,] GeneratePieceContracts(SliceContract sliceContract);
        PieceContract[,] GeneratePieceContracts(SliceContract sliceContract, Action<float> percentageCompleted);
        Mesh ConvertToMappingMesh(MeshContract meshContract);
        Mesh ConvertToBackseatMesh(MeshContract meshContract);
        GameObject GeneratePiece(string name, Vector3 position, Mesh mappingMesh, Mesh backseatMesh, Color color, Texture2D mainTexture, Texture2D normalMap);
        string GeneratePieceName(int row, int column);
        void DestoryAllPieces();
    }
}
