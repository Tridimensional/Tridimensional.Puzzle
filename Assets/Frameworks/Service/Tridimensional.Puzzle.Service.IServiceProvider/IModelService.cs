using System.Collections.Generic;
using Tridimensional.Puzzle.Foundation.Entity;
using Tridimensional.Puzzle.Foundation.Enumeration;
using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
	public interface IModelService
	{
        LayoutContract GetProperLayout(int width, int height, int count);
        LayoutContract GetProperLayout(int width, int height, GameDifficulty gameDifficulty);
        LayoutContract GetProperLayout(Texture2D image, int count);
        LayoutContract GetProperLayout(Texture2D image, GameDifficulty gameDifficulty);
        SliceContract GetSlice(Texture2D image, LayoutContract layoutContract, SliceProgram sliceProgram);
        GameObject GeneratePiece(string name, Vector3 position, Mesh mappingMesh, Mesh backseatMesh, Color color, Texture2D mainTexture, Texture2D normalMap);
        string GeneratePieceName(int row, int column);
        PieceContract[,] GeneratePieceContracts(SliceContract sliceContract);
        Texture2D GenerateNormalMap(SliceContract sliceContract);
    }
}
