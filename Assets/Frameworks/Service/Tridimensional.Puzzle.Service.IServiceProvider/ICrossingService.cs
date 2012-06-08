using Tridimensional.Puzzle.Foundation.Entity;
using Tridimensional.Puzzle.Service.Contract;

namespace Tridimensional.Puzzle.Service.IServiceProvider
{
	public interface ICrossingService
	{
        MultiTree<CrossContract> GetConfigurationTree();
    }
}
