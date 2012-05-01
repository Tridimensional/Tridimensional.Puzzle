using Ninject.Modules;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation;

namespace Tridimensional.Puzzle.IOC.BindingModules
{
	public class ServiceBindingModule : NinjectModule
	{
        public override void Load()
        {
            Bind<IPieceService>().To<PieceService>().InSingletonScope();
            Bind<IModelService>().To<ModelService>().InSingletonScope();
        }
	}
}
