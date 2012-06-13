﻿using UnityEngine;

namespace Tridimensional.Puzzle.Core.Entity
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Point operator *(Point p, float f)
        {
            return new Point(Mathf.RoundToInt(p.X * f), Mathf.RoundToInt(p.Y * f));
        }

        public static Point operator *(float f, Point p)
        {
            return new Point(Mathf.RoundToInt(p.X * f), Mathf.RoundToInt(p.Y * f));
        }

        public static Point operator /(Point p, float f)
        {
            return new Point(Mathf.RoundToInt(p.X / f), Mathf.RoundToInt(p.Y / f));
        }

        public static float Distance(Point p1, Point p2)
        {
            var x = p1.X - p2.X;
            var y = p1.Y - p2.Y;
            return Mathf.Sqrt(x * x + y * y);
        }
    }
}