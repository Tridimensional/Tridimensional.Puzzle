using Tridimensional.Puzzle.Core.Enumeration;

namespace Tridimensional.Puzzle.Service.ServiceImplementation.SliceStrategy
{
	public class SliceStrategyFactory
	{
        public AbstractSliceStrategy Create(SlicePattern slicePattern)
        {
            switch (slicePattern)
            {
                case SlicePattern.Random:
                    return new RandomSliceStrategy();
                default:
                    return new DefaultSliceStrategy();
            }
        }
	}
}
