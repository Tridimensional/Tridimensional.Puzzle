using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tridimensional.Puzzle.Service.Contract
{
	public class LayoutContract
	{
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
