using System;
using Tridimensional.Puzzle.Core.Enumeration;

namespace Tridimensional.Puzzle.Service.ServiceImplementation.ImageSourceStrategy
{
	public class ImageSourceStrategyFactory
	{
        public IImageSourceStrategy Create(ImageSource imageSource)
        {
            switch (imageSource)
            {
                case ImageSource.Local:
                    return new LocalImageStrategy();
                case ImageSource.Official:
                    return new OfficialImageStrategy();
                default:
                    throw new NotImplementedException();
            }
        }
	}
}
