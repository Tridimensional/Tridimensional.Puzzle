using System;
using Tridimensional.Puzzle.Core;
using Tridimensional.Puzzle.Core.Entity;
using Tridimensional.Puzzle.Service.Contract;
using Tridimensional.Puzzle.Service.IServiceProvider;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation
{
    public class GraphicsService : IGraphicsService
    {
        #region Instance

        static IGraphicsService _instance;

        public static IGraphicsService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GraphicsService();
                }
                return _instance;
            }
        }

        #endregion

        public Image GenerateNormalMap(SliceContract sliceContract)
        {
            return GenerateNormalMap(sliceContract, null);
        }

        public Image GenerateNormalMap(SliceContract sliceContract, Action<float> percentageCompleted)
        {
            return GenerateNormalMap(sliceContract, 1, percentageCompleted);
        }

        public Image GenerateNormalMap(SliceContract sliceContract, float strength)
        {
            return GenerateNormalMap(sliceContract, strength, null);
        }

        public Image GenerateNormalMap(SliceContract sliceContract, float strength, Action<float> percentageCompleted)
        {
            var heightMap = null as Image;
            if (percentageCompleted == null) { heightMap = GenerateHeightMap(sliceContract); }
            else { heightMap = GenerateHeightMap(sliceContract, refer => { percentageCompleted(0.9f * refer); }); }

            var dx = 0f; var dy = 0f;
            var left = 0f; var right = 0f; var up = 0f; var down = 0f;

            strength = Mathf.Clamp(strength, 0f, 10f);

            var normalMap = new Image(heightMap.width, heightMap.height);

            for (var i = 0; i < normalMap.width; i++)
            {
                for (var j = 0; j < normalMap.height; j++)
                {
                    left = heightMap.GetPixel(i - 1, j).grayscale * strength;
                    right = heightMap.GetPixel(i + 1, j).grayscale * strength;
                    up = heightMap.GetPixel(i, j - 1).grayscale * strength;
                    down = heightMap.GetPixel(i, j + 1).grayscale * strength;

                    dx = (left - right) * 0.5f;
                    dy = (down - up) * 0.5f;

                    normalMap.SetPixel(i, j, new Color(dx, dy, 1f, dx));
                }
            }

            if (percentageCompleted != null) { percentageCompleted(1); }

            return normalMap;
        }

        public Image GenerateHeightMap(SliceContract sliceContract)
        {
            return GenerateHeightMap(sliceContract, null);
        }

        public Image GenerateHeightMap(SliceContract sliceContract, Action<float> percentageCompleted)
        {
            var heightMap = new Image(sliceContract.Width, sliceContract.Height);

            for (var i = 0; i < heightMap.width; i++)
            {
                for (var j = 0; j < heightMap.height; j++)
                {
                    heightMap.SetPixel(i, j, Color.white);
                }
            }

            var rows = sliceContract.Vertexes.GetLength(0) - 1;
            var columns = sliceContract.Vertexes.GetLength(1) - 1;
            var lines = null as Point[];

            var totalSteps = rows * columns + rows + columns;
            var completedSteps = 0;

            for (var i = 0; i < rows - 1; i++)
            {
                for (var j = 0; j < columns - 1; j++)
                {
                    lines = sliceContract.Lines[i + 1, j, i + 1, j + 1];
                    DrawLine(heightMap, sliceContract.Vertexes[i + 1, j], lines[0]);
                    DrawLine(heightMap, lines);
                    DrawLine(heightMap, lines[lines.Length - 1], sliceContract.Vertexes[i + 1, j + 1]);

                    lines = sliceContract.Lines[i, j + 1, i + 1, j + 1];
                    DrawLine(heightMap, sliceContract.Vertexes[i, j + 1], lines[0]);
                    DrawLine(heightMap, lines);
                    DrawLine(heightMap, lines[lines.Length - 1], sliceContract.Vertexes[i + 1, j + 1]);

                    if (percentageCompleted != null) { percentageCompleted(1f * (++completedSteps) / totalSteps); }
                }
            }

            for (var i = 0; i < rows - 1; i++)
            {
                lines = sliceContract.Lines[i + 1, columns - 1, i + 1, columns];
                DrawLine(heightMap, sliceContract.Vertexes[i + 1, columns - 1], lines[0]);
                DrawLine(heightMap, lines);
                DrawLine(heightMap, lines[lines.Length - 1], sliceContract.Vertexes[i + 1, columns]);
            }

            for (var j = 0; j < columns - 1; j++)
            {
                lines = sliceContract.Lines[rows - 1, j + 1, rows, j + 1];
                DrawLine(heightMap, sliceContract.Vertexes[rows - 1, j + 1], lines[0]);
                DrawLine(heightMap, lines);
                DrawLine(heightMap, lines[lines.Length - 1], sliceContract.Vertexes[rows, j + 1]);
            }

            if (percentageCompleted != null) { percentageCompleted(1); }

            return heightMap;
        }

        private void DrawLine(Image source, Point[] points)
        {
            for (var i = 0; i < points.Length - 1; i++)
            {
                DrawLine(source, points[i], points[i + 1]);
            }
        }

        private void DrawLine(Image source, Point from, Point to)
        {
            DrawSoften(source, from.X, from.Y);
            DrawSoften(source, to.X, to.Y);

            var dx = to.X - from.X;
            var dy = to.Y - from.Y;

            if (dx == 0 && dy == 0) { return; }

            var start = 0; var end = 0; var estimate = 0f;
            var horizontal = Mathf.Abs(dx) > Mathf.Abs(dy);
            var delta = horizontal ? (float)dy / dx : (float)dx / dy;

            if (horizontal)
            {
                if (dx > 0) { start = from.X; end = to.X; estimate = from.Y; }
                else { start = to.X; end = from.X; estimate = to.Y; }
            }
            else
            {
                if (dy > 0) { start = from.Y; end = to.Y; estimate = from.X; }
                else { start = to.Y; end = from.Y; estimate = to.X; }
            }

            while (start <= end)
            {
                if (horizontal) { DrawSoften(source, start, estimate); }
                else { DrawSoften(source, estimate, start); }

                start++;
                estimate += delta;
            }
        }

        private void DrawSoften(Image source, int x, int y)
        {
            var width = GlobalConfiguration.SoftenWidthInPixel;

            for (var i = -width; i <= width; i++)
            {
                for (var j = -width; j <= width; j++)
                {
                    SetPixel(source, x + i, y + j, GetColor(Mathf.Sqrt(i * i + j * j)));
                }
            }
        }

        private void DrawSoften(Image source, int x, float y)
        {
            var width = GlobalConfiguration.SoftenWidthInPixel;
            var round = Mathf.RoundToInt(y);

            for (var i = -width; i <= width + 1; i++)
            {
                SetPixel(source, x, round + i, GetColor(Mathf.Abs(round + i - y)));
            }
        }

        private void DrawSoften(Image source, float x, int y)
        {
            var width = GlobalConfiguration.SoftenWidthInPixel;
            var round = Mathf.RoundToInt(x);

            for (var i = -width; i <= width + 1; i++)
            {
                SetPixel(source, round + i, y, GetColor(Mathf.Abs(round + i - x)));
            }
        }

        private void SetPixel(Image source, int x, int y, Color color)
        {
            if (x < 0 || x > source.width || y < 0 || y > source.height) { return; }
            if (color.r > source.GetPixel(x, y).r) { return; }
            source.SetPixel(x, y, color);
        }

        private Color GetColor(float distance)
        {
            var width = GlobalConfiguration.SoftenWidthInPixel;
            if (distance >= width + 1) { return Color.white; }

            var value = Mathf.Sqrt(distance / (width + 1));

            return new Color(value, value, value);
        }
    }
}
