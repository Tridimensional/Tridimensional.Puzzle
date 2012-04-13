using Tridimensional.Puzzle.Foundation.Entity;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.Contract
{
	public class SliceContract
	{
        public Vector2[,] Vertexes { get; set; }
        public LineDictionary Lines { get; set; }
	}
}
