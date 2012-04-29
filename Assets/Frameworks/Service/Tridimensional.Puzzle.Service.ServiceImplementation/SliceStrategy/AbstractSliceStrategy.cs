using System;
using Tridimensional.Puzzle.Foundation.Entity;
using Tridimensional.Puzzle.Foundation.Utility;
using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation.SliceStrategy
{
    public abstract class AbstractSliceStrategy
    {
        public abstract SliceContract GetSlice(LayoutContract layoutContract);
        public abstract Vector2[,] GetVertexes(LayoutContract layoutContract);
        public abstract Vector2[] GetConnectPoints(bool needFlip);

        public LineDictionary GetLines(Vector2[,] vertexes)
        {
            var lineDictionary = new LineDictionary();
            var rows = vertexes.GetLength(0);
            var columns = vertexes.GetLength(1);

            for (var i = 0; i < rows - 1; i++)
            {
                for (var j = 0; j < columns - 1; j++)
                {
                    SetLineDictionary(lineDictionary, i, j, i + 1, j, vertexes);
                    SetLineDictionary(lineDictionary, i, j, i, j + 1, vertexes);
                }
            }

            for (var i = 0; i < rows - 1; i++)
            {
                SetLineDictionary(lineDictionary, i, columns - 1, i + 1, columns - 1, vertexes);
            }

            for (var j = 0; j < columns - 1; j++)
            {
                SetLineDictionary(lineDictionary, rows - 1, j, rows - 1, j + 1, vertexes);
            }

            return lineDictionary;
        }

        private void SetLineDictionary(LineDictionary lineDictionary, int x1, int y1, int x2, int y2, Vector2[,] vertexes)
        {
            if ((x1 == 0 && x2 == 0) || (y1 == 0 && y2 == 0)) { lineDictionary[x1, y1, x2, y2] = null; return; }
            var rows = vertexes.GetLength(0);
            var columns = vertexes.GetLength(1);
            if ((x1 == x2 && x1 == rows - 1) || (y1 == y2 && y1 == columns - 1)) { lineDictionary[x1, y1, x2, y2] = null; return; }

            lineDictionary[x1, y1, x2, y2] = GetConnectPoints(vertexes[x1, y1], vertexes[x2, y2], (x1 + y1) % 2 == 0);
        }

        private Vector2[] GetConnectPoints(Vector2 head, Vector2 tail, bool needFlip)
        {
            var points = GetConnectPoints(needFlip);
            var angle = VectorUtility.GetAngle(points[points.Length - 1] - points[0], tail - head);
            var zoom = Vector2.Distance(tail, head) / Vector2.Distance(points[points.Length - 1], points[0]);
            var result = new Vector2[points.Length - 2];

            for (var i = 1; i < points.Length - 1; i++)
            {
                result[i - 1] = Transform(points[0], points[i], angle, zoom, head - points[0]);
            }

            return result;
        }

        private Vector2 Transform(Vector2 start, Vector2 end, float degrees, float zoom, Vector2 translation)
        {
            return Transform(end - start, degrees, zoom, start + translation);
        }

        private Vector2 Transform(Vector2 point, float degrees, float zoom, Vector2 translation)
        {
            var angle = Math.PI * degrees / 180;
            var sin = (float)Math.Sin(angle);
            var cos = (float)Math.Cos(angle);
            
            return new Vector2(point.x * cos - point.y * sin, point.x * sin + point.y * cos) * zoom + translation;
        }
    }
}
