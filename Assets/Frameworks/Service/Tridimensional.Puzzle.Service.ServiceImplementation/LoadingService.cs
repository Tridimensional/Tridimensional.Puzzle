using Tridimensional.Puzzle.Service.IServiceProvider;

namespace Tridimensional.Puzzle.Service.ServiceImplementation
{
	public class LoadingService : ILoadingService
	{
        #region Instance

        static ILoadingService _instance;

        public static ILoadingService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LoadingService();
                }
                return _instance;
            }
        }

        #endregion
    }
}
