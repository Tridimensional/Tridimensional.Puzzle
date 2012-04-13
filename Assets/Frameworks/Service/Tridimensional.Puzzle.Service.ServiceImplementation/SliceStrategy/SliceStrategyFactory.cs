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
                default:
                    return new DefaultSliceStrategy();
            }
        }
	}
}
