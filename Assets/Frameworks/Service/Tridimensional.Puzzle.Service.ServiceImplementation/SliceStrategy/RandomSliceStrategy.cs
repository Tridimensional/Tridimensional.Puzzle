﻿using System;
using Tridimensional.Puzzle.Foundation.Entity;
using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation.SliceStrategy
{
    public class RandomSliceStrategy : AbstractSliceStrategy
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
            var randomRange = layoutContract.Height / layoutContract.Rows / 10;

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    vertexes[i, j] = new Point(layoutContract.Width * j / layoutContract.Columns, layoutContract.Height * i / layoutContract.Rows) + GetRandomOffset(j != 0 && j != columns - 1, i != 0 && i != rows - 1, randomRange);
                }
            }

            return vertexes;
        }

        public override Point[] GetConnectPoints(bool needFlip)
        {
            var points = new[]
            {
                new Point(0,0),
                new Point(500,100),
                new Point(1000,0)
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
            return Convert.ToInt32(range * (1 - UnityEngine.Random.value * 2));
        }
    }
}
