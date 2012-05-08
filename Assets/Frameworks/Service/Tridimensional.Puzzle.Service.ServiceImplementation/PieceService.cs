using System;
using System.Collections.Generic;
using Tridimensional.Puzzle.Foundation;
using Tridimensional.Puzzle.Foundation.Enumeration;
using Tridimensional.Puzzle.Foundation.Utility;
using Tridimensional.Puzzle.Service.Contract;
using Tridimensional.Puzzle.Service.IServiceProvider;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation
{
	public class PieceService : IPieceService
    {
        public PieceContract[,] GeneratePiece(SliceContract sliceContract, Vector2 mappingOffset)
        {
            var rows = sliceContract.Vertexes.GetLength(0) - 1;
            var columns = sliceContract.Vertexes.GetLength(1) - 1;
            var result = new PieceContract[rows, columns];

            var start = sliceContract.Vertexes[0, 0];
            var end = sliceContract.Vertexes[rows, columns];
            var range = new Vector2(end.x - start.x, end.y - start.y);

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    var vertexes = GetVertexes(sliceContract, i, j);
                    var center = new Vector2(range.x * (2 * j + 1f) / (2 * columns), range.y * (2 * i + 1f) / (2 * rows));

                    var topVertexes = Array.ConvertAll<Vector2, Vector3>(vertexes, refer => new Vector3(refer.x - center.x, refer.y - center.y, -GlobalConfiguration.PieceThicknessInMeter / 2));
                    var bottomVertexes = Array.ConvertAll<Vector3, Vector3>(topVertexes, refer => new Vector3(refer.x, refer.y, refer.z + GlobalConfiguration.PieceThicknessInMeter));

                    var topVertexeContracts = ConvertToVertexContracts(topVertexes, 0);
                    var bottomVertexeContracts = ConvertToVertexContracts(bottomVertexes, topVertexeContracts.Length);

                    var upperTriangles = GetTriangles(topVertexeContracts.Copy());
                    var sideTriangles = GetTriangles(topVertexeContracts.Copy(), bottomVertexeContracts.Copy(), Shape.Cylinder);
                    var bottomTriangles = GetTriangles(bottomVertexeContracts.Copy(), true);

                    var pieceContract = new PieceContract { MappingMesh = new Mesh(), BackseatMesh = new Mesh(), Position = center - range / 2 };

                    pieceContract.MappingMesh.vertices = topVertexes;
                    pieceContract.MappingMesh.triangles = GetTriangles(upperTriangles);
                    pieceContract.MappingMesh.uv = GetUVs(vertexes, mappingOffset, start, range);
                    pieceContract.MappingMesh.RecalculateNormals();

                    pieceContract.BackseatMesh.vertices = GetVertexes(topVertexeContracts, bottomVertexeContracts);
                    pieceContract.BackseatMesh.triangles = GetTriangles(sideTriangles, bottomTriangles);
                    pieceContract.BackseatMesh.normals = GetNormals(pieceContract.BackseatMesh.vertices.Length);

                    result[i, j] = pieceContract;
                }
            }

            return result;
        }

        private Vector2[] GetVertexes(SliceContract sliceContract, int x, int y)
        {
            var points = null as Vector2[];
            var vertexes = new List<Vector2>();

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

            return vertexes.ToArray();
        }

        private Vector2[] GetUVs(Vector2[] vertexes, Vector2 offset, Vector2 start, Vector2 range)
        {
            var vectors = new Vector2[vertexes.Length];

            for (var i = 0; i < vertexes.Length; i++)
            {
                vectors[i] = new Vector2((vertexes[i].x + offset.x - start.x) / (range.x + offset.x * 2), (vertexes[i].y + offset.y - start.y) / (range.y + offset.y * 2));
            }

            return vectors;
        }

        private Vector3[] GetNormals(int length)
        {
            var normals = new Vector3[length];
            for (var i = 0; i < length; i++) { normals[i] = new Vector3(0, 0, 1); }
            return normals;
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
                triangles.Add(pointer1.Next.Index);
                triangles.Add(pointer2.Index);
                triangles.Add(pointer2.Next.Index);

                pointer1 = pointer1.Next;
                pointer2 = pointer2.Next;
            }

            triangles.Add(pointer1.Index);
            triangles.Add(pointer2.Index);
            triangles.Add(pointer1.Next.Index);
            triangles.Add(pointer1.Next.Index);
            triangles.Add(pointer2.Index);
            triangles.Add(pointer2.Next.Index);

            return triangles;
        }

        private VertexContract[] ConvertToVertexContracts(Vector3[] vector3s, int startIndex)
        {
            return Array.ConvertAll<Vector3, VertexContract>(vector3s, refer => refer.ToVertexContract(startIndex++));
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
