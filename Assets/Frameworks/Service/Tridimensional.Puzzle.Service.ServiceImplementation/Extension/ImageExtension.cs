using UnityEngine;

namespace Tridimensional.Puzzle.Core.Entity
{
	public static class ImageExtension
	{
        public static Texture2D ToTexture2D(this Image image)
        {
            var texture = new Texture2D(image.width, image.height);

            for (var i = 0; i < image.width; i++)
            {
                for (var j = 0; j < image.height; j++)
                {
                    texture.SetPixel(i, j, image.GetPixel(i, j));
                }
            }

            texture.Apply();

            return texture;
        }
	}
}
