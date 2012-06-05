using Ninject;
using Tridimensional.Puzzle.IOC.BindingModules;

namespace Tridimensional.Puzzle.IOC
{
    public class InjectionRepository
    {
        #region Instance

        static InjectionRepository _instance;
        static readonly object _padLock = new object();

        public static InjectionRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_padLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new InjectionRepository();
                        }
                    }
                }

                return _instance;
            }
        }

        #endregion

        readonly IKernel _kernel;

        public IKernel Kernel
        {
            get { return this._kernel; }
        }

        public InjectionRepository()
        {
            _kernel = new StandardKernel(new ServiceBindingModule());
        }

        public T Get<T>()
        {
            return _kernel.Get<T>();
        }
    }
}
