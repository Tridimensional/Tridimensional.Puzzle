using Tridimensional.Puzzle.Foundation.Entity;
using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation.SliceStrategy
{
    public class DefaultSliceStrategy : AbstractSliceStrategy
    {
        public override SliceContract GetSlice(LayoutContract layoutContract)
        {
            var vertexes = GetVertexes(layoutContract);
            var lines = GetLines(vertexes);
            return new SliceContract { Vertexes = vertexes, Lines = lines };
        }

        public override Point[,] GetVertexes(LayoutContract layoutContract)
        {
            var rows = layoutContract.Rows + 1;
            var columns = layoutContract.Columns + 1;
            var vertexes = new Point[rows, columns];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    vertexes[i, j] = new Point(layoutContract.Width * j / layoutContract.Columns, layoutContract.Height * i / layoutContract.Rows);
                }
            }

            return vertexes;
        }

        public override Point[] GetConnectPoints(bool needFlip)
        {
            var points = new[]
            {
                new Point(0,0),
                new Point(250,0),
                new Point(500,200),
                new Point(750,0),
                new Point(1000,0)
            };

            if (needFlip)
            {
                for (var i = 0; i < points.Length; i++) { points[i].Y *= -1; }
            }

            return points;
        }
    }
}
