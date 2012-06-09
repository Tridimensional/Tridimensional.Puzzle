using Tridimensional.Puzzle.Core.Enumeration;
using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
	public interface IModelService
	{
        LayoutContract GetProperLayout(int width, int height, int count);
        LayoutContract GetProperLayout(int width, int height, Difficulty difficulty);
        LayoutContract GetProperLayout(Texture2D image, int count);
        LayoutContract GetProperLayout(Texture2D image, Difficulty difficulty);
        SliceContract GetSlice(Texture2D image, LayoutContract layoutContract, SlicePattern slicePattern);
        GameObject GeneratePiece(string name, Vector3 position, Mesh mappingMesh, Mesh backseatMesh, Color color, Texture2D mainTexture, Texture2D normalMap);
        string GeneratePieceName(int row, int column);
        PieceContract[,] GeneratePieceContracts(SliceContract sliceContract);
        Texture2D GenerateNormalMap(SliceContract sliceContract);
    }
}
