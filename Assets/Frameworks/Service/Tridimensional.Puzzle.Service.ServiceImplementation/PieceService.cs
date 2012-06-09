using System;
using System.Collections.Generic;
using Tridimensional.Puzzle.Core;
using Tridimensional.Puzzle.Core.Entity;
using Tridimensional.Puzzle.Core.Enumeration;
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
                    mappingMesh.RecalculateNormals();

                    var backseatMesh = new Mesh();
                    backseatMesh.vertices = GetVertices(backseatSurfaces);
                    backseatMesh.triangles = GetTriangles(backseatSurfaces);
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
            points = Point.Reverse(sliceContract.Lines[x, y + 1, x + 1, y + 1]);
            if (points != null) { for (var i = 0; i < points.Length; i++) { vertexes.Add(points[i].ToVector2(conversionRate)); } }

            vertexes.Add(sliceContract.Vertexes[x, y + 1].ToVector2(conversionRate));
            points = Point.Reverse(sliceContract.Lines[x, y, x, y + 1]);
            if (points != null) { for (var i = 0; i < points.Length; i++) { vertexes.Add(points[i].ToVector2(conversionRate)); } }

            return vertexes.ToArray();
        }

        private List<SurfaceContract> GetSurfaces(Vector3[] topVertexes, Vector3[] bottomVertexes)
        {
            var endIndex = 0;
            var surfaces = new List<SurfaceContract>();

            for (var i = 0; i < topVertexes.Length; i++)
            {
                endIndex = i < topVertexes.Length - 1 ? i + 1 : 0;
                surfaces.Join(ConvertToVertexContracts(topVertexes[i], bottomVertexes[i], bottomVertexes[endIndex], topVertexes[endIndex]), Direction.Side);
            }

            surfaces.Join(ConvertToVertexContracts(bottomVertexes), Direction.Back);

            return surfaces;
        }

        private Vector3[] GetVertices(List<SurfaceContract> surfaces)
        {
            var length = 0;
            surfaces.ForEach(refer => length += refer.Vertexes.Length);

            var index = 0;
            var result = new Vector3[length];

            foreach (var surface in surfaces)
            {
                for (var i = 0; i < surface.Vertexes.Length; i++)
                {
                    result[index++] = surface.Vertexes[i].ToVector3();
                }
            }

            return result;
        }

        private int[] GetTriangles(List<SurfaceContract> surfaces)
        {
            var temp = null as List<int>;
            var list = new List<List<int>>();
            var vertexes = null as VertexContract[];
            var length = 0;

            foreach (var surface in surfaces)
            {
                vertexes = surface.Vertexes;

                switch (surface.Direction)
                {
                    case Direction.Side:
                        temp = new List<int> { vertexes[0].Index, vertexes[1].Index, vertexes[2].Index, vertexes[0].Index, vertexes[2].Index, vertexes[3].Index };
                        break;
                    case Direction.Back:
                        temp = GetTriangles(vertexes, true);
                        break;
                    default:
                        temp = GetTriangles(vertexes, false);
                        break;
                }

                list.Add(temp);
                length += temp.Count;
            }

            var index = 0;
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
            return GetTriangles(ConvertToVertexContracts(vertexes).Link(0), false).ToArray();
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

                if (reverse)
                {
                    triangles.Add(pointer.Next.Index);
                    triangles.Add(pointer.Index);
                    triangles.Add(pointer.Previous.Index);
                }
                else
                {
                    triangles.Add(pointer.Previous.Index);
                    triangles.Add(pointer.Index);
                    triangles.Add(pointer.Next.Index);
                }

                pointer.Next.Previous = pointer.Previous;
                pointer.Previous.Next = pointer.Next;
                pointer = pointer.Next;
            }

            if (reverse)
            {
                triangles.Add(pointer.Next.Index);
                triangles.Add(pointer.Index);
                triangles.Add(pointer.Previous.Index);
            }
            else
            {
                triangles.Add(pointer.Previous.Index);
                triangles.Add(pointer.Index);
                triangles.Add(pointer.Next.Index);
            }

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

        private VertexContract[] ConvertToVertexContracts(params Vector3[] vector3s)
        {
            return Array.ConvertAll<Vector3, VertexContract>(vector3s, refer => refer.ToVertexContract());
        }

        private float GetIncludeAngle(VertexContract vertex)
        {
            return VectorUtility.GetAngle((vertex.Previous.Position - vertex.Position).ToVector2(), (vertex.Next.Position - vertex.Position).ToVector2());
        }

        private bool ExistsIncludedVertex(VertexContract vertex)
        {
            var a = vertex.Position.ToVector2();
            var b = vertex.Previous.Position.ToVector2();
            var c = vertex.Next.Position.ToVector2();

            var pointer = vertex.Next.Next;

            while (pointer.Next.Index != vertex.Index)
            {
                var o = pointer.Position.ToVector2();
                if (VectorUtility.OntheSameSide(o, c, a, b) && VectorUtility.OntheSameSide(o, a, b, c) && VectorUtility.OntheSameSide(o, b, a, c)) { return true; }
                pointer = pointer.Next;
            }

            return false;
        }
    }
}
