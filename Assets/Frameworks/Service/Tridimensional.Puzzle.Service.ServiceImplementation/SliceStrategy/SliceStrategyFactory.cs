using Tridimensional.Puzzle.Core.Enumeration;

namespace Tridimensional.Puzzle.Service.ServiceImplementation.SliceStrategy
{
    public class SliceStrategyFactory
    {
        #region Instance

        private static SliceStrategyFactory _instance;

        public static SliceStrategyFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SliceStrategyFactory();
                }
                return _instance;
            }
        }

        #endregion

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
