using System;
using Tridimensional.Puzzle.Foundation.Entity;
using Tridimensional.Puzzle.Foundation.Utility;
using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation.SliceStrategy
{
    public abstract class AbstractSliceStrategy
    {
        protected abstract Point[,] GetVertexes(LayoutContract layoutContract, Point offset);
        protected abstract Point[] GetConnectPoints(bool needFlip);

        public SliceContract GetSlice(Texture2D image, LayoutContract layoutContract)
        {
            var scale = GetSliceScale(image, layoutContract);
            var offset = (scale - new Point(layoutContract.Width, layoutContract.Height)) / 2;
            var vertexes = GetVertexes(layoutContract, offset);
            var lines = GetLines(vertexes);

            return new SliceContract { Vertexes = vertexes, Lines = lines, Width = scale.X, Height = scale.Y };
        }

        protected LineDictionary GetLines(Point[,] vertexes)
        {
            var lineDictionary = new LineDictionary();
            var rows = vertexes.GetLength(0);
            var columns = vertexes.GetLength(1);

            for (var i = 0; i < rows - 1; i++)
            {
                for (var j = 0; j < columns - 1; j++)
                {
                    if (i == 0) { SetLineDictionary(lineDictionary, i, j, i, j + 1); }
                    else { SetLineDictionary(lineDictionary, i, j, i, j + 1, vertexes); }
                    if (j == 0) { SetLineDictionary(lineDictionary, i, j, i + 1, j); }
                    else { SetLineDictionary(lineDictionary, i, j, i + 1, j, vertexes); }
                }
            }

            for (var i = 0; i < rows - 1; i++) { SetLineDictionary(lineDictionary, i, columns - 1, i + 1, columns - 1); }
            for (var j = 0; j < columns - 1; j++) { SetLineDictionary(lineDictionary, rows - 1, j, rows - 1, j + 1); }

            return lineDictionary;
        }

        private Point GetSliceScale(Texture2D image, LayoutContract layoutContract)
        {
            if (image.width * layoutContract.Height > image.height * layoutContract.Width)
            {
                return new Point(image.width * layoutContract.Height / image.height, layoutContract.Height);
            }
            else
            {
                return new Point(layoutContract.Width, image.height * layoutContract.Width / image.width);
            }
        }

        private void SetLineDictionary(LineDictionary lineDictionary, int x1, int y1, int x2, int y2)
        {
            lineDictionary[x1, y1, x2, y2] = null;
        }

        private void SetLineDictionary(LineDictionary lineDictionary, int x1, int y1, int x2, int y2, Point[,] vertexes)
        {
            lineDictionary[x1, y1, x2, y2] = GetConnectPoints(vertexes[x1, y1], vertexes[x2, y2], (x1 + y1) % 2 == 0);
        }

        private Point[] GetConnectPoints(Point head, Point tail, bool needFlip)
        {
            var points = GetConnectPoints(needFlip);
            var angle = VectorUtility.GetAngle(points[points.Length - 1] - points[0], tail - head);
            var zoom = Point.Distance(tail, head) / Point.Distance(points[points.Length - 1], points[0]);
            var result = new Point[points.Length - 2];

            for (var i = 1; i < points.Length - 1; i++)
            {
                result[i - 1] = Transform(points[0], points[i], angle, zoom, head - points[0]);
            }

            return result;
        }

        private Point Transform(Point start, Point end, float degrees, float zoom, Point translation)
        {
            return Transform(end - start, degrees, zoom, start + translation);
        }

        private Point Transform(Point point, float degrees, float zoom, Point translation)
        {
            var angle = Mathf.PI * degrees / 180;
            var sin = Mathf.Sin(angle);
            var cos = Mathf.Cos(angle);

            return new Point(Convert.ToInt32(point.X * cos - point.Y * sin), Convert.ToInt32(point.X * sin + point.Y * cos)) * zoom + translation;
        }
    }
}
