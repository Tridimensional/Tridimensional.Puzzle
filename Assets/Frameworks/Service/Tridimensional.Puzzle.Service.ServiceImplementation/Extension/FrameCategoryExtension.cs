using Tridimensional.Puzzle.Service.Contract;

namespace Tridimensional.Puzzle.Foundation.Enumeration
{
	public static  class FrameCategoryExtension
	{
        public static FrameContract ToFrameContract(this FrameCategory frameCategory)
        {
            return new FrameContract { Category = frameCategory };
        }
	}
}
