using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tridimensional.Puzzle.Service.Contract
{
	public class FormationContract
	{
        public int Rows { get; set; }
        public int Columns { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
	}
}
