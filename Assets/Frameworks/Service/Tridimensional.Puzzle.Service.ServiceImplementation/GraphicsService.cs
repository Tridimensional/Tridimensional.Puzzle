using Tridimensional.Puzzle.Foundation.Entity;
using Tridimensional.Puzzle.Service.Contract;
using Tridimensional.Puzzle.Service.IServiceProvider;
using UnityEngine;
using System;

namespace Tridimensional.Puzzle.Service.ServiceImplementation
{
    public class GraphicsService : IGraphicsService
    {
        public Texture2D GenerateNormalMap(SliceContract sliceContract)
        {
            var normalMap = new Texture2D(sliceContract.Width, sliceContract.Height);

            for (var i = 0; i < normalMap.width; i++)
            {
                for (var j = 0; j < normalMap.height; j++)
                {
                    normalMap.SetPixel(i, j, Color.white);
                }
            }

            var rows = sliceContract.Vertexes.GetLength(0) - 1;
            var columns = sliceContract.Vertexes.GetLength(1) - 1;

            for (var i = 0; i < rows - 1; i++)
            {
                for (var j = 0; j < columns - 1; j++)
                {
                    SetPixel(normalMap, sliceContract.Lines[i + 1, j, i + 1, j + 1]);
                    SetPixel(normalMap, sliceContract.Lines[i, j + 1, i + 1, j + 1]);
                }
            }

            for (var i = 0; i < rows - 1; i++)
            {
                SetPixel(normalMap, sliceContract.Lines[i + 1, columns - 1, i + 1, columns]);
            }

            for (var j = 0; j < columns - 1; j++)
            {
                SetPixel(normalMap, sliceContract.Lines[rows - 1, j + 1, rows, j + 1]);
            }

            normalMap.Apply();

            return normalMap;
        }

        private void SetPixel(Texture2D normalMap, Point[] points)
        {
            for (var i = 0; i < points.Length - 1; i++)
            {
                SetPixel(normalMap, points[i].X, points[i].Y, points[i + 1].X, points[i + 1].Y);
            }
        }

        private void SetPixel(Texture2D normalMap, int x1, int y1, int x2, int y2)
        {
            int x = x1;
            int y = y1;
            int dx = Abs(x2 - x1);
            int dy = Abs(y2 - y1);
            int s1 = x2 > x1 ? 1 : -1;
            int s2 = y2 > y1 ? 1 : -1;

            bool interchange = false;

            if (dy > dx)
            {
                int temp = dx;
                dx = dy;
                dy = temp;
                interchange = true;
            }

            int p = 2 * dy - dx;
            for (int i = 0; i < dx; i++)
            {
                SetPixel(normalMap, x, y, Color.black);

                if (p >= 0)
                {
                    if (!interchange)		// 当斜率 < 1 时，选取上下象素点
                        y += s2;
                    else					// 当斜率 > 1 时，选取左右象素点
                        x += s1;
                    p -= 2 * dx;
                }
                if (!interchange)
                    x += s1;				// 当斜率 < 1 时，选取 x 为步长
                else
                    y += s2;				// 当斜率 > 1 时，选取 y 为步长
                p += 2 * dy;
            }
        }

        private void SetPixel(Texture2D normalMap, int x, int y, Color color)
        {
            normalMap.SetPixel(x, y, color);
        }

        private int Abs(int i)
        {
            return i > 0 ? i : -1;
        }
    }
}
