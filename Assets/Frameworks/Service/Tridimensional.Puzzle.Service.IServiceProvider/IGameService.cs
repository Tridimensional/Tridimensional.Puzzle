using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
	public interface IGameService
	{
        GameContract CurrentGame { get; }
        GameContract OpenNew();
        Texture2D GetMainTexture();
        Texture2D GetNormalMap();
        PieceContract[,] GetPieceContracts();
        void Apply();
    }
}
