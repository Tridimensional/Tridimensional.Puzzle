using System;

namespace Tridimensional.Puzzle.Core.Enumeration
{
	public static class DifficultyExtension
	{
        public static int ToProperPuzzleCount(this Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    return 50;
                case Difficulty.Middle:
                    return 80;
                case Difficulty.Hard:
                    return 150;
                default:
                    throw new NotImplementedException();
            }
        }
	}
}
