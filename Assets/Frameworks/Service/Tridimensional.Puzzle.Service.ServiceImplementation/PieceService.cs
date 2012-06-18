using System;
using System.Collections.Generic;
using Tridimensional.Puzzle.Core;
using Tridimensional.Puzzle.Core.Entity;
using Tridimensional.Puzzle.Core.Enumeration;
using Tridimensional.Puzzle.Service.Contract;
using Tridimensional.Puzzle.Service.IServiceProvider;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation
{
    public class PieceService : IPieceService
    {
        #region Instance

        static IPieceService _instance;

        public static IPieceService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PieceService();
                }
                return _instance;
            }
        }

        #endregion

        public PieceContract[,] GeneratePieceContracts(SliceContract sliceContract)
        {
            return GeneratePieceContracts(sliceContract, null);
        }

        public PieceContract[,] GeneratePieceContracts(SliceContract sliceContract, Action<float> percentComplet)
        {
            var rows = sliceContract.Vertexes.GetLength(0) - 1;
            var columns = sliceContract.Vertexes.GetLength(1) - 1;
            var pieceCount = (float)rows * columns;

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
                    var triangulator = new Triangulator(vertexes);
                    var center = new Vector2(offset.x + (j + 0.5f) * pieceRange.x, offset.y + (i + 0.5f) * pieceRange.y);

                    var topVertexes = Array.ConvertAll<Vector2, Vector3>(vertexes, refer => new Vector3(refer.x - center.x, refer.y - center.y, -GlobalConfiguration.PieceThicknessInMeter / 2));
                    var bottomVertexes = Array.ConvertAll<Vector3, Vector3>(topVertexes, refer => new Vector3(refer.x, refer.y, refer.z + GlobalConfiguration.PieceThicknessInMeter));

                    var mappingMesh = new Mesh();
                    mappingMesh.vertices = topVertexes;
                    mappingMesh.triangles = triangulator.Triangulate();
                    mappingMesh.uv = GetUVs(vertexes, range);
                    mappingMesh.RecalculateNormals();
                    //mappingMesh.ReCalculateTangents();

                    var backseatMesh = new Mesh();
                    backseatMesh.vertices = Merge<Vector3>(bottomVertexes, GetSideVertices(topVertexes, bottomVertexes));
                    backseatMesh.triangles = Merge<int>(Reverse<int>(mappingMesh.triangles), GetSideTriangles(vertexes.Length));
                    backseatMesh.RecalculateNormals();

                    result[i, j] = new PieceContract { MappingMesh = mappingMesh, BackseatMesh = backseatMesh, Position = center - range / 2 };

                    if (percentComplet != null) { percentComplet((i * columns + j + 1) / pieceCount); }
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
            points = Reverse<Point>(sliceContract.Lines[x, y + 1, x + 1, y + 1]);
            if (points != null) { for (var i = 0; i < points.Length; i++) { vertexes.Add(points[i].ToVector2(conversionRate)); } }

            vertexes.Add(sliceContract.Vertexes[x, y + 1].ToVector2(conversionRate));
            points = Reverse<Point>(sliceContract.Lines[x, y, x, y + 1]);
            if (points != null) { for (var i = 0; i < points.Length; i++) { vertexes.Add(points[i].ToVector2(conversionRate)); } }

            return vertexes.ToArray();
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

        private Vector3[] GetSideVertices(Vector3[] topVertexes, Vector3[] bottomVertexes)
        {
            var result = new Vector3[topVertexes.Length * 4];

            for (var i = 0; i < topVertexes.Length - 1; i++)
            {
                result[i * 4] = topVertexes[i];
                result[i * 4 + 1] = bottomVertexes[i];
                result[i * 4 + 2] = bottomVertexes[i + 1];
                result[i * 4 + 3] = topVertexes[i + 1];
            }

            result[result.Length - 4] = topVertexes[topVertexes.Length - 1];
            result[result.Length - 3] = bottomVertexes[topVertexes.Length - 1];
            result[result.Length - 2] = bottomVertexes[0];
            result[result.Length - 1] = topVertexes[0];

            return result;
        }

        private int[] GetSideTriangles(int basis)
        {
            var result = new int[basis * 6];
            var key = 0;
            var value = 0;

            for (var i = 0; i < basis; i++)
            {
                key = i * 6;
                value = basis + i * 4;

                result[key] = value;
                result[key + 1] = value + 1;
                result[key + 2] = value + 2;
                result[key + 3] = value;
                result[key + 4] = value + 2;
                result[key + 5] = value + 3;
            }

            return result;
        }

        private T[] Reverse<T>(T[] args)
        {
            if (args == null) { return null; }

            var result = new T[args.Length];

            for (var i = 0; i < args.Length; i++)
            {
                result[i] = args[args.Length - 1 - i];
            }

            return result;
        }

        private T[] Merge<T>(params T[][] args)
        {
            if (args == null) { return null; }

            var list = new List<T>();

            for (var i = 0; i < args.Length; i++)
            {
                for (var j = 0; j < args[i].Length; j++)
                {
                    list.Add(args[i][j]);
                }
            }

            return list.ToArray();
        }

        public GameObject GeneratePiece(string name, Vector3 position, Mesh mappingMesh, Mesh backseatMesh, Color color, Texture2D mainTexture, Texture2D normalMap)
        {
            return GeneratePiece(name, position, Quaternion.Euler(0, 0, 0), mappingMesh, backseatMesh, color, mainTexture, normalMap);
        }

        public GameObject GeneratePiece(string name, Vector3 position, Quaternion rotation, Mesh mappingMesh, Mesh backseatMesh, Color color, Texture2D mainTexture, Texture2D normalMap)
        {
            var go = new GameObject(name);
            go.AddComponent<MeshFilter>().mesh = backseatMesh;
            go.AddComponent<MeshRenderer>().material.color = color;
            go.transform.position = position;
            go.transform.localRotation = rotation;
            go.tag = CustomTags.Piece.ToString();

            var mapping = new GameObject("Mapping");
            mapping.AddComponent<MeshFilter>().mesh = mappingMesh;
            mapping.AddComponent<MeshRenderer>().material = Resources.Load("Material/BumpedDiffuse") as Material;
            mapping.transform.renderer.material.SetTexture("_MainTex", mainTexture);
            mapping.transform.renderer.material.SetTexture("_BumpMap", normalMap);

            mapping.transform.parent = go.transform;
            mapping.transform.localPosition = new Vector3(0, 0, 0);
            mapping.transform.localRotation = Quaternion.Euler(0, 0, 0);

            return go;
        }

        public string GeneratePieceName(int row, int column)
        {
            return string.Format("Piece <{0:000},{1:000}>", row, column);
        }

        public void DestoryAllPieces()
        {
            var pieces = GameObject.FindGameObjectsWithTag(CustomTags.Piece.ToString());

            foreach (var piece in pieces)
            {
                GameObject.Destroy(piece);
            }
        }
    }
}
