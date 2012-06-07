using UnityEngine;
using Tridimensional.Puzzle.Foundation.Entity;

namespace Tridimensional.Puzzle.Service.Contract
{
    public static class VertexContractExtension
    {
        public static Vector3 ToVector3(this VertexContract vertex)
        {
            return new Vector3 { x = vertex.x, y = vertex.y, z = vertex.z };
        }

        public static Vector2 ToVector2(this VertexContract vertex)
        {
            return new Vector2 { x = vertex.x, y = vertex.y };
        }

        public static VertexContract[] Link(this VertexContract[] source, int startIndex)
        {
            for (var i = 0; i < source.Length; i++)
            {
                source[i].Previous = i == 0 ? source[source.Length - 1] : source[i - 1];
                source[i].Next = i == source.Length - 1 ? source[0] : source[i + 1];
                source[i].Index = startIndex++;
            }

            return source;
        }

        public static VertexContract[] Reverse(this VertexContract[] source)
        {
            if (source == null) { return null; }
            var result = new VertexContract[source.Length];
            for (var i = 0; i < source.Length; i++) { result[i] = source[source.Length - i - 1]; }
            return result;
        }
    }
}
