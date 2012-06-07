using System;
using System.Collections.Generic;
using Tridimensional.Puzzle.Foundation;
using Tridimensional.Puzzle.Foundation.Entity;
using Tridimensional.Puzzle.Foundation.Utility;
using Tridimensional.Puzzle.Service.Contract;
using Tridimensional.Puzzle.Service.IServiceProvider;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation
{
    public class PieceService : IPieceService
    {
        public PieceContract[,] GeneratePiece(SliceContract sliceContract)
        {
            //var testVectors = new[]
            //{
            //    new Vector3(0, 0,0),
            //    new Vector3(250, 0,0),
            //    new Vector3(500, 100,0),
            //    new Vector3(750, 0,0),
            //    new Vector3(1000, 0,0)
            //};


            //var surfaces2 = new List<VertexContract[]>();
            //JoinSurface(surfaces2, ConvertToVertexContracts(testVectors), true);
            //var triangles2 = GetTriangles(surfaces2);

            //Debug.Log(">>>>>>>>>>>>>>>>>>>>> reverse <<<<<<<<<<<<<<<<<<<<<<");
            //foreach (var t in triangles2)
            //{
            //    Debug.Log(t);
            //}

            //var surfaces1 = new List<VertexContract[]>();
            //JoinSurface(surfaces1, ConvertToVertexContracts(testVectors), false);
            //var triangles1 = GetTriangles(surfaces1);

            //Debug.Log(">>>>>>>>>>>>>>>>>>>>> forward <<<<<<<<<<<<<<<<<<<<<<");
            //foreach (var t in triangles1)
            //{
            //    Debug.Log(t);
            //}
            







            var rows = sliceContract.Vertexes.GetLength(0) - 1;
            var columns = sliceContract.Vertexes.GetLength(1) - 1;

            var sliceStart = sliceContract.Vertexes[0, 0];
            var sliceValidRange = sliceContract.Vertexes[rows, columns] - sliceStart;
            var conversionRate = GlobalConfiguration.PictureHeightInMeter / sliceValidRange.Y;

            var range = new Vector2(sliceContract.Width, sliceContract.Height) * conversionRate;
            var pieceRange = new Vector2(1f * sliceValidRange.X / columns, 1f * sliceValidRange.Y / rows) * conversionRate;
            var offset = new Vector2(sliceStart.X, sliceStart.Y) * conversionRate;

            var result = new PieceContract[rows, columns];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    var vertexes = GetVertexes(sliceContract, i, j, conversionRate);
                    var center = new Vector2(offset.x + (j + 0.5f) * pieceRange.x, offset.y + (i + 0.5f) * pieceRange.y);

                    var topVertexes = Array.ConvertAll<Vector2, Vector3>(vertexes, refer => new Vector3(refer.x - center.x, refer.y - center.y, -GlobalConfiguration.PieceThicknessInMeter / 2));
                    var bottomVertexes = Array.ConvertAll<Vector3, Vector3>(topVertexes, refer => new Vector3(refer.x, refer.y, refer.z + GlobalConfiguration.PieceThicknessInMeter));

                    var backseatSurfaces = GetSurfaces(topVertexes, bottomVertexes);

                    var mappingMesh = new Mesh();
                    mappingMesh.vertices = topVertexes;
                    mappingMesh.triangles = GetTriangles(topVertexes);
                    mappingMesh.uv = GetUVs(vertexes, range);
                    mappingMesh.tangents = new Vector4[mappingMesh.vertexCount];
                    mappingMesh.RecalculateNormals();

                    var backseatMesh = new Mesh();
                    backseatMesh.vertices = GetVertices(backseatSurfaces);
                    backseatMesh.triangles = GetTriangles(backseatSurfaces);
                    backseatMesh.uv = new Vector2[backseatMesh.vertexCount];
                    backseatMesh.RecalculateNormals();

                    result[i, j] = new PieceContract { MappingMesh = mappingMesh, BackseatMesh = backseatMesh, Position = center - range / 2 };
                }
            }

            return result;
        }

        private Vector2[] GetVertexes(SliceContract sliceContract, int x, int y, float conversionRate)
        {
            var vertexes = new List<Vector2>();

            vertexes.Add(sliceContract.Vertexes[x, y].ToVector2(conversionRate));
            var points = sliceContract.Lines[x, y, x + 1, y];
            if (points != null) { for (var i = 0; i < points.Length; i++) { vertexes.Add(points[i].ToVector2(conversionRate)); } }

            vertexes.Add(sliceContract.Vertexes[x + 1, y].ToVector2(conversionRate));
            points = sliceContract.Lines[x + 1, y, x + 1, y + 1];
            if (points != null) { for (var i = 0; i < points.Length; i++) { vertexes.Add(points[i].ToVector2(conversionRate)); } }

            vertexes.Add(sliceContract.Vertexes[x + 1, y + 1].ToVector2(conversionRate));
            points = VectorUtility.Reverse(sliceContract.Lines[x, y + 1, x + 1, y + 1]);
            if (points != null) { for (var i = 0; i < points.Length; i++) { vertexes.Add(points[i].ToVector2(conversionRate)); } }

            vertexes.Add(sliceContract.Vertexes[x, y + 1].ToVector2(conversionRate));
            points = VectorUtility.Reverse(sliceContract.Lines[x, y, x, y + 1]);
            if (points != null) { for (var i = 0; i < points.Length; i++) { vertexes.Add(points[i].ToVector2(conversionRate)); } }

            return vertexes.ToArray();
        }

        private List<VertexContract[]> GetSurfaces(Vector3[] topVertexes, Vector3[] bottomVertexes)
        {
            var endIndex = 0;
            var surfaces = new List<VertexContract[]>();

            for (var i = 0; i < topVertexes.Length; i++)
            {
                endIndex = i < topVertexes.Length - 1 ? i + 1 : 0;
                JoinSurface(surfaces, ConvertToVertexContracts(topVertexes[i], bottomVertexes[i], bottomVertexes[endIndex], topVertexes[endIndex]));
            }

            JoinSurface(surfaces, ConvertToVertexContracts(bottomVertexes));

            return surfaces;
        }

        private Vector3[] GetVertices(List<VertexContract[]> surfaces)
        {
            var length = 0;
            surfaces.ForEach(refer => length += refer.Length);

            var index = 0;
            var result = new Vector3[length];

            foreach (var surface in surfaces)
            {
                for (var i = 0; i < surface.Length; i++)
                {
                    result[index++] = surface[i].ToVector3();
                }
            }

            return result;
        }

        private int[] GetTriangles(List<VertexContract[]> surfaces)
        {
            var length = 0;
            var list = new List<List<int>>();

            foreach (var surface in surfaces)
            {
                var _list = GetTriangles(surface);
                length += _list.Count;
                list.Add(_list);
            }

            var index =0;
            var result = new int[length];

            for (var i = 0; i < list.Count; i++)
            {
                for (var j = 0; j < list[i].Count; j++)
                {
                    result[index++] = list[i][j];
                }
            }

            return result;
        }

        private int[] GetTriangles(Vector3[] vertexes)
        {
            return GetTriangles(ConvertToVertexContracts(vertexes).Link(0)).ToArray();
        }

        private List<int> GetTriangles(VertexContract[] vertexes)
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

                triangles.Add(pointer.Previous.Index);
                triangles.Add(pointer.Index);
                triangles.Add(pointer.Next.Index);

                pointer.Next.Previous = pointer.Previous;
                pointer.Previous.Next = pointer.Next;
                pointer = pointer.Next;
            }

            triangles.Add(pointer.Previous.Index);
            triangles.Add(pointer.Index);
            triangles.Add(pointer.Next.Index);

            return triangles;
        }

        private Vector2[] GetUVs(Vector2[] vertexes, Vector2 range)
        {
            var vectors = new Vector2[vertexes.Length];

            for (var i = 0; i < vertexes.Length; i++)
            {
                vectors[i] = new Vector2((vertexes[i].x) / range.x, (vertexes[i].y) / range.y);
            }

            return vectors;
        }

        private void JoinSurface(List<VertexContract[]> surfaces, VertexContract[] vertexes)
        {
            if (vertexes != null) { surfaces.Add(vertexes.Link(GetStartIndex(surfaces))); }
        }

        private int GetStartIndex(List<VertexContract[]> surfaces)
        {
            var startIndex = 0;
            foreach (var surface in surfaces) { startIndex += surface == null ? 0 : surface.Length; }
            return startIndex;
        }

        private VertexContract[] ConvertToVertexContracts(params Vector3[] vector3s)
        {
            return Array.ConvertAll<Vector3, VertexContract>(vector3s, refer => refer.ToVertexContract());
        }

        private float GetIncludeAngle(VertexContract vertex)
        {
            return VectorUtility.GetAngle(new Vector2(vertex.Previous.x - vertex.x, vertex.Previous.y - vertex.y), new Vector2(vertex.Next.x - vertex.x, vertex.Next.y - vertex.y));
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
