using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation.ImageSourceStrategy
{
	public interface IImageSourceStrategy
	{
        Texture2D GetImage(GameContract gameContract);
    }
}
