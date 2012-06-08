using System.Collections.Generic;
using Tridimensional.Puzzle.Foundation.Enumeration;

namespace Tridimensional.Puzzle.Service.Contract
{
    public static class SurfaceContractExtension
    {
        public static void Join(this List<SurfaceContract> surfaceContracts, VertexContract[] vertexes, Direction direction)
        {
            surfaceContracts.Add(new SurfaceContract { Vertexes = vertexes.Link(GetVectexesCount(surfaceContracts)), Direction = direction });
        }

        public static int GetVectexesCount(this List<SurfaceContract> surfaceContracts)
        {
            var count = 0;
            foreach (var refer in surfaceContracts) { count += refer.Vertexes == null ? 0 : refer.Vertexes.Length; }
            return count;
        }
    }
}
