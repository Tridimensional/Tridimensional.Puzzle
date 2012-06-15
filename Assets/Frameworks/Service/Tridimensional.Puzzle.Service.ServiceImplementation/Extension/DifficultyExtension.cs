namespace Tridimensional.Puzzle.Core.Enumeration
{
	public static class DifficultyExtension
	{
        public static int ToProperPuzzleCount(this Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    return 20;
                case Difficulty.Middle:
                    return 50;
                case Difficulty.Hard:
                    return 5000;
                default:
                    return 200;
            }
        }
	}
}
