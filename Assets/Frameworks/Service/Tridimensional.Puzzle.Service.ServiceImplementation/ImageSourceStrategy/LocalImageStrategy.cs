using System;
using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation.ImageSourceStrategy
{
    public class LocalImageStrategy : IImageSourceStrategy
    {
        public Texture2D GetImage(GameContract gameContract)
        {
            return Resources.Load(gameContract.ImageAddress) as Texture2D;
        }
    }
}
