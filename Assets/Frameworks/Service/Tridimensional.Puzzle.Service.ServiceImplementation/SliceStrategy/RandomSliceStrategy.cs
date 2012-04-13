using System;
using Tridimensional.Puzzle.Service.Contract;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation.SliceStrategy
{
    public class RandomSliceStrategy : AbstractSliceStrategy
    {
        public override SliceContract GetSlice(FormationContract formationContract)
        {
            var vertexes = GetVertexes(formationContract);
            var lines = GetLines(vertexes);
            return new SliceContract { Vertexes = vertexes, Lines = lines };
        }

        public override Vector2[,] GetVertexes(FormationContract formation)
        {
            var vertexes = new Vector2[formation.Rows + 1, formation.Columns + 1];
            var rows = vertexes.GetLength(0);
            var columns = vertexes.GetLength(1);
            var randomScale = formation.Width / formation.Columns / 12;

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    vertexes[i, j] = new Vector2(1.0f * formation.Width * j / formation.Columns - formation.Width / 2, 1.0f * formation.Height * i / formation.Rows - formation.Height / 2) + GetRandomOffset(j != 0 && j != columns - 1, i != 0 && i != rows - 1, randomScale);
                }
            }

            return vertexes;
        }

        public override Vector2[] GetConnectPoints(bool inverted)
        {
            var points = new[] {
                new Vector2(0f,0f),
                new Vector2(0.02173913f,0f),
                new Vector2(0.1222826f, 0.02445652f),
                new Vector2(0.2690217f, 0.04891304f),
                new Vector2(0.375f, 0.04347826f),
                new Vector2(0.3967391f, 0.0326087f),
                new Vector2(0.4130435f, 0.01630435f),
                new Vector2(0.4184783f, 0f),
                new Vector2(0.4157609f, -0.01630435f),
                new Vector2(0.3858696f, -0.04619565f),
                new Vector2(0.3695652f, -0.0625f),
                new Vector2(0.3614131f, -0.08695652f),
                new Vector2(0.3586957f, -0.1168478f),
                new Vector2(0.3668478f, -0.1467391f),
                new Vector2(0.3831522f, -0.1684783f),
                new Vector2(0.4211957f, -0.1929348f),
                new Vector2(0.4646739f, -0.2119565f),
                new Vector2(0.5353261f, -0.2119565f),
                new Vector2(0.5788044f, -0.1929348f),
                new Vector2(0.6168478f, -0.1684783f),
                new Vector2(0.6331522f, -0.1467391f),
                new Vector2(0.6413044f, -0.1168478f),
                new Vector2(0.638587f, -0.08695652f),
                new Vector2(0.6304348f, -0.0625f),
                new Vector2(0.6141304f, -0.04619565f),
                new Vector2(0.5842391f, -0.01630435f),
                new Vector2(0.5815217f, 0f),
                new Vector2(0.5869566f, 0.01630435f),
                new Vector2(0.6032609f, 0.0326087f),
                new Vector2(0.625f, 0.04347826f),
                new Vector2(0.7309783f, 0.04891304f),
                new Vector2(0.8777174f, 0.02445652f),
                new Vector2(0.9782609f, 0f),
                new Vector2(1f, 0f)
            };

            if (UnityEngine.Random.value < 0.5)
            {
                for (var i = 0; i < points.Length; i++) { points[i].y *= -1; }
            }

            return points;
        }

        private Vector2 GetRandomOffset(bool horizontal, bool vertical, float scale)
        {
            return new Vector2(horizontal ? GetRandomOffset(scale) : 0f, vertical ? GetRandomOffset(scale) : 0f);
        }

        private float GetRandomOffset(float scale)
        {
            return scale * (1f - UnityEngine.Random.value * 2f);
        }
    }
}
