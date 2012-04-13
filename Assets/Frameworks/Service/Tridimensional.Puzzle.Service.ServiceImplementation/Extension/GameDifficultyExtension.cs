namespace Tridimensional.Puzzle.Foundation.Enumeration
{
	public static class GameDifficultyExtension
	{
        public static int ToProperPuzzleCount(this GameDifficulty gameDifficulty)
        {
            switch (gameDifficulty)
            {
                case GameDifficulty.Easy:
                    return 20;
                case GameDifficulty.Middle:
                    return 50;
                case GameDifficulty.Hard:
                    return 100;
                default:
                    return 200;
            }
        }
	}
}
