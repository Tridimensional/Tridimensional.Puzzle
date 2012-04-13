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

        public static VertexContract[] Copy(this VertexContract[] source)
        {
            var result = new VertexContract[source.Length];

            for (var i = 0; i < source.Length; i++)
            {
                result[i] = new VertexContract { x = source[i].x, y = source[i].y, z = source[i].z, Index = source[i].Index };
            }

            for (var i = 0; i < result.Length; i++)
            {
                result[i].Previous = i == 0 ? result[result.Length - 1] : result[i - 1];
                result[i].Next = i == result.Length - 1 ? result[0] : result[i + 1];
            }

            return result;
        }
	}
}
