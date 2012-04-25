using System;
using System.Collections.Generic;
using Tridimensional.Puzzle.Foundation.Enumeration;
using Tridimensional.Puzzle.Foundation.Utility;
using Tridimensional.Puzzle.Service.Contract;
using Tridimensional.Puzzle.Service.IServiceProvider;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation
{
	public class MeshService : IMeshService
    {
		//total width of puzzle
		private float pzWidth;
		//total height of puzzle
		private float pzHeight;
		//start x of puzzle
		private float startX;
		//start y of puzzle
		private float startY;
		
        public Mesh[,] GenerateMesh(SliceContract sliceContract)
        {
			pzWidth = sliceContract.Vertexes[0,sliceContract.Vertexes.GetLength(1) - 1].x - sliceContract.Vertexes[0,0].x;
			pzHeight = sliceContract.Vertexes[sliceContract.Vertexes.GetLength(0) - 1,0].y - sliceContract.Vertexes[0,0].y;
			startX = sliceContract.Vertexes[0,0].x;
			startY = sliceContract.Vertexes[0,0].y;
			
            var rows = sliceContract.Vertexes.GetLength(0) - 1;
            var columns = sliceContract.Vertexes.GetLength(1) - 1;
            var result = new Mesh[rows, columns];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    result[i, j] = GenerateMesh(sliceContract, i, j);
                }
            }

            return result;
        }

        private Mesh GenerateMesh(SliceContract sliceContract, int x, int y)
        {
            var vertexes = new List<Vector2>();
            var points = null as Vector2[];

            vertexes.Add(sliceContract.Vertexes[x, y]);
            points = sliceContract.Lines[x, y, x + 1, y];
            if (points != null) { foreach (var p in points) { vertexes.Add(p); } }

            vertexes.Add(sliceContract.Vertexes[x + 1, y]);
            points = sliceContract.Lines[x + 1, y, x + 1, y + 1];
            if (points != null) { foreach (var p in points) { vertexes.Add(p); } }

            vertexes.Add(sliceContract.Vertexes[x + 1, y + 1]);
            points = VectorUtility.Reverse(sliceContract.Lines[x, y + 1, x + 1, y + 1]);
            if (points != null) { foreach (var p in points) { vertexes.Add(p); } }

            vertexes.Add(sliceContract.Vertexes[x, y + 1]);
            points = VectorUtility.Reverse(sliceContract.Lines[x, y, x, y + 1]);
            if (points != null) { foreach (var p in points) { vertexes.Add(p); } }

            return GenerateMesh(vertexes.ToArray());
        }

        private Mesh GenerateMesh(Vector2[] vertexes)
        {
            var mesh = new Mesh();

            var topVertexes = Array.ConvertAll<Vector2, Vector3>(vertexes, refer => new Vector3(refer.x, refer.y, 0));
            var bottomVertexes = Array.ConvertAll<Vector2, Vector3>(vertexes, refer => new Vector3(refer.x, refer.y, 0.003f));

            var topVertexeContracts = ConvertToVertexContracts(topVertexes, 0);
            var bottomVertexeContract = ConvertToVertexContracts(bottomVertexes, topVertexeContracts.Length);

            var upperTriangles = GetTriangles(topVertexeContracts.Copy());
            var sideTriangles = GetTriangles(topVertexeContracts.Copy(), bottomVertexeContract.Copy(), Shape.Cylinder);
            var bottomTriangles = GetTriangles(bottomVertexeContract.Copy(), true);

            mesh.vertices = GetVertexes(topVertexeContracts, bottomVertexeContract);
            mesh.triangles = GetTriangles(upperTriangles, sideTriangles, bottomTriangles);
			mesh.uv = GetUVs(topVertexeContracts,bottomVertexeContract.Length);
			
            return mesh;
        }
		
		private Vector2[] GetUVs(VertexContract[] vertexes,int backVertexCount)
		{
			Vector2[] UVValue = new Vector2[vertexes.Length + backVertexCount];
			for(int i=0; i < vertexes.Length; i++)
			{
				UVValue[i] = new Vector2((vertexes[i].x - startX)/pzWidth,(vertexes[i].y - startY)/pzHeight);
			}
			
			return UVValue;
		}

        private Vector3[] GetVertexes(params VertexContract[][] vertexes)
        {
            var length = 0;
            for (var i = 0; i < vertexes.Length; i++) { length += vertexes[i].Length; }

            var result = new Vector3[length];

            for (var i = 0; i < vertexes.Length; i++)
            {
                for (var j = 0; j < vertexes[i].Length; j++)
                {
                    result[vertexes[i][j].Index] = vertexes[i][j].ToVector3();
                }
            }

            return result;
        }

        private int[] GetTriangles(params List<int>[] indexes)
        {
            var length = 0;
            for (var i = 0; i < indexes.Length; i++) { length += indexes[i].Count; }

            var index = 0;
            var result = new int[length];

            for (var i = 0; i < indexes.Length; i++)
            {
                for (var j = 0; j < indexes[i].Count; j++)
                {
                    result[index++] = indexes[i][j];
                }
            }

            return result;
        }

        private List<int> GetTriangles(VertexContract[] vertexes)
        {
            return GetTriangles(vertexes, false);
        }

        private List<int> GetTriangles(VertexContract[] vertexes, bool reverse)
        {
            var pointer = vertexes[0];
            var triangles = new List<int>();

            for (var i = 0; i < vertexes.Length - 3; i++)
            {
                var index = pointer.Index;

                while (GetIncludeAngle(pointer) >= 180.0f || ExistsIncludedVertex(pointer))
                {
                    pointer = pointer.Next;
                    if (pointer.Index == index) { break; }
                }

                triangles.Add(reverse ? pointer.Next.Index : pointer.Previous.Index);
                triangles.Add(pointer.Index);
                triangles.Add(reverse ? pointer.Previous.Index : pointer.Next.Index);

                pointer.Next.Previous = pointer.Previous;
                pointer.Previous.Next = pointer.Next;
                pointer = pointer.Next;
            }

            triangles.Add(reverse ? pointer.Next.Index : pointer.Previous.Index);
            triangles.Add(pointer.Index);
            triangles.Add(reverse ? pointer.Previous.Index : pointer.Next.Index);

            return triangles;
        }

        private List<int> GetTriangles(VertexContract[] vertexes1, VertexContract[] vertexes2, Shape shape)
        {
            var pointer1 = vertexes1[0];
            var pointer2 = vertexes2[0];
            var triangles = new List<int>();

            if (shape == Shape.Torus)
            {
                return triangles;
            }

            while (pointer1.Next.Index != vertexes1[0].Index)
            {
                triangles.Add(pointer1.Index);
                triangles.Add(pointer2.Index);
                triangles.Add(pointer1.Next.Index);
                triangles.Add(pointer2.Index);
                triangles.Add(pointer2.Next.Index);
                triangles.Add(pointer1.Next.Index);

                pointer1 = pointer1.Next;
                pointer2 = pointer2.Next;
            }

            triangles.Add(pointer1.Index);
            triangles.Add(pointer2.Index);
            triangles.Add(pointer1.Next.Index);
            triangles.Add(pointer2.Index);
            triangles.Add(pointer2.Next.Index);
            triangles.Add(pointer1.Next.Index);

            return triangles;
        }

        private VertexContract[] ConvertToVertexContracts(Vector3[] vector3s, int startIndex)
        {
            var vertexContracts = Array.ConvertAll<Vector3, VertexContract>(vector3s, refer => refer.ToVertexContract(startIndex++));

            for (var i = 0; i < vertexContracts.Length; i++)
            {
                vertexContracts[i].Previous = i == 0 ? vertexContracts[vertexContracts.Length - 1] : vertexContracts[i - 1];
                vertexContracts[i].Next = i == vertexContracts.Length - 1 ? vertexContracts[0] : vertexContracts[i + 1];
            }

            return vertexContracts;
        }

        private float GetIncludeAngle(VertexContract vertex)
        {
            var from = new Vector2(vertex.Previous.x - vertex.x, vertex.Previous.y - vertex.y);
            var to = new Vector2(vertex.Next.x - vertex.x, vertex.Next.y - vertex.y);

            return VectorUtility.GetAngle(from, to);
        }

        private bool ExistsIncludedVertex(VertexContract vertex)
        {
            var a = new Vector2(vertex.x, vertex.y);
            var b = new Vector2(vertex.Previous.x, vertex.Previous.y);
            var c = new Vector2(vertex.Next.x, vertex.Next.y);

            var pointer = vertex.Next.Next;

            while (pointer.Next.Index != vertex.Index)
            {
                var o = new Vector2(pointer.x, pointer.y);
                if (VectorUtility.OntheSameSide(o, c, a, b) && VectorUtility.OntheSameSide(o, a, b, c) && VectorUtility.OntheSameSide(o, b, a, c)) { return true; }
                pointer = pointer.Next;
            }

            return false;
        }
    }
}
