using Tridimensional.Puzzle.Service.Contract;

namespace Tridimensional.Puzzle.Core
{
    public class GameCommander
	{
        static GameContract _gameInstance;

        public static GameContract OpenNew()
        {
            return _gameInstance = new GameContract();
        }

        public static GameContract GameInstance
        {
            get { return _gameInstance; }
        }
	}
}
