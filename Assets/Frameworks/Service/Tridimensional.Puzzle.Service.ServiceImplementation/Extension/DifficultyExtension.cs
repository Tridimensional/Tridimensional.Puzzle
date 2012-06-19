using System;

namespace Tridimensional.Puzzle.Core.Enumeration
{
	public static class DifficultyExtension
	{
        public static int ToProperPuzzleCount(this Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Simple:
                    return 50;
                case Difficulty.Medium:
                    return 80;
                case Difficulty.Difficult:
                    return 150;
                default:
                    throw new NotImplementedException();
            }
        }
	}
}
