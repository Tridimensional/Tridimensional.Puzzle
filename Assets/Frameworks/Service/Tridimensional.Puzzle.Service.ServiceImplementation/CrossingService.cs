using Tridimensional.Puzzle.Service.IServiceProvider;

namespace Tridimensional.Puzzle.Service.ServiceImplementation
{
    public class CrossingService : ICrossingService
    {
        #region Instance

        static ICrossingService _instance;

        public static ICrossingService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CrossingService();
                }
                return _instance;
            }
        }

        #endregion
    }
}
