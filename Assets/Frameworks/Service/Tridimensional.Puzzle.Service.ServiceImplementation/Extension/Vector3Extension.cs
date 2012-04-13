using System;
using Tridimensional.Puzzle.Service.Contract;

namespace UnityEngine
{
    public static class Vector3Extension
    {
        public static VertexContract ToVertexContract(this Vector3 vector3, int index)
        {
            return new VertexContract { x = vector3.x, y = vector3.y, z = vector3.z, Index = index };
        }
    }
}
