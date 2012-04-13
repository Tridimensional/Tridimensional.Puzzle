using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tridimensional.Puzzle.Foundation.Enumeration;

namespace Tridimensional.Puzzle.Service.ServiceImplementation.SliceStrategy
{
	public class SliceStrategyFactory
	{
        public AbstractSliceStrategy Create(SliceProgram sliceProgram)
        {
            switch (sliceProgram)
            {
                case SliceProgram.Random:
                    return new RandomSliceStrategy();
                default:
                    return new DefaultSliceStrategy();
            }
        }
	}
}
