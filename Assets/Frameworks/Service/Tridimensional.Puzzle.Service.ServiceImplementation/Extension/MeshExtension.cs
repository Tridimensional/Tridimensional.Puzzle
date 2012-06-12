using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
	public static class MeshExtension
	{
        public static Mesh CalculateTangents(this Mesh mesh)
        {
            var triangleCount = mesh.triangles.Length / 3;
            var vertexCount = mesh.vertices.Length;

            var tan1 = new Vector3[vertexCount];
            var tan2 = new Vector3[vertexCount];

            var tangents = new Vector4[vertexCount];

            for (var a = 0; a < triangleCount; a += 3)
            {
                var i1 = mesh.triangles[a + 0];
                var i2 = mesh.triangles[a + 1];
                var i3 = mesh.triangles[a + 2];

                var v1 = mesh.vertices[i1];
                var v2 = mesh.vertices[i2];
                var v3 = mesh.vertices[i3];

                var w1 = mesh.uv[i1];
                var w2 = mesh.uv[i2];
                var w3 = mesh.uv[i3];

                var x1 = v2.x - v1.x;
                var x2 = v3.x - v1.x;
                var y1 = v2.y - v1.y;
                var y2 = v3.y - v1.y;
                var z1 = v2.z - v1.z;
                var z2 = v3.z - v1.z;

                var s1 = w2.x - w1.x;
                var s2 = w3.x - w1.x;
                var t1 = w2.y - w1.y;
                var t2 = w3.y - w1.y;

                var r = 1.0f / (s1 * t2 - s2 * t1);

                var sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
                var tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

                tan1[i1] += sdir;
                tan1[i2] += sdir;
                tan1[i3] += sdir;

                tan2[i1] += tdir;
                tan2[i2] += tdir;
                tan2[i3] += tdir;
            }


            for (long a = 0; a < vertexCount; ++a)
            {
                var n = mesh.normals[a];
                var t = tan1[a];

                var tmp = (t - n * Vector3.Dot(n, t)).normalized;
                tangents[a] = new Vector4(tmp.x, tmp.y, tmp.z);

                tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
            }

            mesh.tangents = tangents;

            return mesh;
        }
	}
}
