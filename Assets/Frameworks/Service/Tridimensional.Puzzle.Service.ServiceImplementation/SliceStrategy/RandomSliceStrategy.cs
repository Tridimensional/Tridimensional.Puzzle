using Tridimensional.Puzzle.Core.Entity;
using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation.SliceStrategy
{
    public class RandomSliceStrategy : AbstractSliceStrategy
    {
        protected override Point[,] GetVertexes(LayoutContract layoutContract, Point offset)
        {
            var rows = layoutContract.Rows + 1;
            var columns = layoutContract.Columns + 1;
            var vertexes = new Point[rows, columns];
            var randomRange = layoutContract.Height / layoutContract.Rows / 10;

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    vertexes[i, j] = offset + new Point(layoutContract.Width * j / layoutContract.Columns, layoutContract.Height * i / layoutContract.Rows) + GetRandomOffset(j != 0 && j != columns - 1, i != 0 && i != rows - 1, randomRange);
                }
            }

            return vertexes;
        }

        protected override Point[] GetConnectPoints(bool needFlip)
        {
            var points = new[]
            {
                new Point(0, 0),
                new Point(22, 0),
                new Point(122, 24),
                new Point(269, 49),
                new Point(375, 43),
                new Point(397, 33),
                new Point(413, 16),
                new Point(418, 0),
                new Point(416, -16),
                new Point(386, -46),
                new Point(370, -62),
                new Point(361, -87),
                new Point(359, -117),
                new Point(367, -147),
                new Point(383, -168),
                new Point(421, -193),
                new Point(465, -212),
                new Point(535, -212),
                new Point(579, -193),
                new Point(617, -168),
                new Point(633, -147),
                new Point(641, -117),
                new Point(639, -87),
                new Point(630, -62),
                new Point(614, -46),
                new Point(584, -16),
                new Point(582, 0),
                new Point(587, 16),
                new Point(603, 33),
                new Point(625, 43),
                new Point(731, 49),
                new Point(878, 24),
                new Point(978, 0),
                new Point(1000, 0)
            };

            if (UnityEngine.Random.value < 0.5)
            {
                for (var i = 0; i < points.Length; i++) { points[i].Y *= -1; }
            }

            return points;
        }

        private Point GetRandomOffset(bool horizontal, bool vertical, int range)
        {
            return new Point(horizontal ? GetRandomOffset(range) : 0, vertical ? GetRandomOffset(range) : 0);
        }

        private int GetRandomOffset(int range)
        {
            return Mathf.RoundToInt(range * (1 - UnityEngine.Random.value * 2));
        }
    }
}
