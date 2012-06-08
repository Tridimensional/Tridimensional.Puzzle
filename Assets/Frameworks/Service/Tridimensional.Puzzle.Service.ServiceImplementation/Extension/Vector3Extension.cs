using Tridimensional.Puzzle.Service.Contract;

namespace UnityEngine
{
    public static class Vector3Extension
    {
        public static VertexContract ToVertexContract(this Vector3 vector3)
        {
            return new VertexContract { Position = vector3 };
        }

        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.y);
        }
    }
}
