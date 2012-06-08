using Tridimensional.Puzzle.Service.Contract;

namespace Tridimensional.Puzzle.Foundation.Enumeration
{
	public static class ButtonCategoryExtension
	{
        public static ButtonContract ToButtonContract(this ButtonCategory buttonCategory)
        {
            var buttonContract = new ButtonContract { Category = buttonCategory };

            switch (buttonCategory)
            {
                case ButtonCategory.CreateNew:
                    buttonContract.Text = "Create New";
                    break;
                default:
                    buttonContract.Text = "Unknown";
                    break;
            }

            return buttonContract;
        }
	}
}
