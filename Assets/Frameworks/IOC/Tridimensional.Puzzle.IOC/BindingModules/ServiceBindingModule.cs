using Ninject.Modules;
using Tridimensional.Puzzle.Service.IServiceProvider;
using Tridimensional.Puzzle.Service.ServiceImplementation;

namespace Tridimensional.Puzzle.IOC.BindingModules
{
	public class ServiceBindingModule : NinjectModule
	{
        public override void Load()
        {
            Bind<IAudioService>().To<AudioService>().InSingletonScope();
            Bind<ICrossingService>().To<CrossingService>().InSingletonScope();
            Bind<IPieceService>().To<PieceService>().InSingletonScope();
            Bind<IGraphicsService>().To<GraphicsService>().InSingletonScope();
            Bind<IModelService>().To<ModelService>().InSingletonScope();
        }
	}
}
