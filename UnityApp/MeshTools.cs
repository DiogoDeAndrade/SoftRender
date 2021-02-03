using SoftRender.Engine;
using System.Collections.Generic;

namespace SoftRender.UnityApp
{
    static public class MeshTools
    {

        public static void ComputeNormals(Mesh mesh, bool area_weight)
        {
            var indices = mesh.GetTriangles();
            var vertices = mesh.GetVertices();

            List<Vector3> triangle_normals = new List<Vector3>();
            List<float> triangle_areas = new List<float>();
            List<List<int>> triangles_per_vertex = new List<List<int>>();

            for (int i = 0; i < indices.Length; i += 3)
            {
                int i1, i2, i3;
                i1 = indices[i];
                i2 = indices[i + 1];
                i3 = indices[i + 2];

                Vector3 v1, v2, v3;
                v1 = vertices[i1];
                v2 = vertices[i2];
                v3 = vertices[i3];

                Vector3 normal = Vector3.Cross(v2 - v1, v3 - v1);
                triangle_areas.Add(normal.magnitude * 0.5f);
                triangle_normals.Add(normal.normalized);

                triangles_per_vertex[i1].Add((int)i / 3);
                triangles_per_vertex[i2].Add((int)i / 3);
                triangles_per_vertex[i3].Add((int)i / 3);
            }

            List<Vector3> normals = new List<Vector3>();

            if (area_weight)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector3 normal = Vector3.zero;
                    float Length = 0.0f;

                    foreach (var index in triangles_per_vertex[i])
                    { 
                        normal += triangle_normals[index] * triangle_areas[index];
                        Length += triangle_areas[index];
                    }

                    normals[i] = normal.normalized;
                }
            }
            else
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector3 normal = Vector3.zero;
                    float Length = 0.0f;

                    foreach (var index in triangles_per_vertex[i])
                    {
                        normal += triangle_normals[index];
                        Length += 1.0f;
                    }

                    normals[i] = normal.normalized;
                }
            }

            mesh.SetNormals(normals);
        }

        public static void ComputeNormalsAndTangentSpace(Mesh mesh, bool area_weight)
        {
            var indices = mesh.GetTriangles();
            var vertices = mesh.GetVertices();
            var uv = mesh.GetUV();

            List<Vector3> triangle_normals = new List<Vector3>();
            List<Vector3> triangle_tangents = new List<Vector3>();
            List<Vector3> triangle_binormals = new List<Vector3>();
            List<float> triangle_areas = new List<float>();
            List<List<int>> triangles_per_vertex = new List<List<int>>();

            for (int i = 0; i < indices.Length; i += 3)
            {
                int i0, i1, i2;
                i0 = indices[i];
                i1 = indices[i + 1];
                i2 = indices[i + 2];

                Vector3 v0, v1, v2;
                Vector2 uv0, uv1, uv2;
                v0 = vertices[i0]; uv0 = uv[i0];
                v1 = vertices[i1]; uv1 = uv[i1];
                v2 = vertices[i2]; uv2 = uv[i2];

                var side0 = v0 - v1;
                var side1 = v2 - v1;

                // Normal and triangle area
                Vector3 normal = Vector3.Cross(side1, side0);
                triangle_areas.Add(normal.magnitude * 0.5f);
                normal = normal.normalized;
                triangle_normals.Add(normal);

                // Tangent space
                Vector2 delta0 = uv0 - uv1;
                Vector2 delta1 = uv2 - uv1;

                Vector3 tangent = (delta1.y * side0 - delta0.y * side1).normalized;
                Vector3 binormal = (delta1.x * side0 - delta0.x * side1).normalized;

                var tangent_cross = Vector3.Cross(tangent, binormal);
                if (Vector3.Dot(tangent_cross, normal) < 0.0f)
                {
                    tangent = -tangent;
                    binormal = -binormal;
                }

                triangle_binormals.Add(binormal);
                triangle_tangents.Add(tangent);

                triangles_per_vertex[i0].Add((int)i / 3);
                triangles_per_vertex[i1].Add((int)i / 3);
                triangles_per_vertex[i2].Add((int)i / 3);
            }

            List<Vector3> normals = new List<Vector3>();
            List<Vector3> tangents = new List<Vector3>();
            List<Vector3> binormals = new List<Vector3>();

            if (area_weight)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector3 normal = Vector3.zero;
                    Vector3 tangent= Vector3.zero;
                    Vector3 binormal= Vector3.zero;
                    float Length = 0.0f;

                    foreach (var index in triangles_per_vertex[i])
                    {
                        normal += triangle_normals[index] * triangle_areas[index];
                        tangent += triangle_tangents[index] * triangle_areas[index];
                        binormal += triangle_binormals[index] * triangle_areas[index];
                        Length += triangle_areas[index];
                    }

                    normals[i] = normal.normalized;
                    tangents[i] = tangent.normalized;
                    binormals[i] = binormal.normalized;
                }
            }
            else
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector3 normal= Vector3.zero;
                    Vector3 tangent= Vector3.zero;
                    Vector3 binormal= Vector3.zero;
                    float Length = 0.0f;

                    foreach (var index in triangles_per_vertex[i])
                    {
                        normal += triangle_normals[index];
                        tangent += triangle_tangents[index];
                        binormal += triangle_binormals[index];
                        Length += 1.0f;
                    }

                    normals[i] = normal.normalized;
                    tangents[i] = tangent.normalized;
                    binormals[i] = binormal.normalized;
                }
            }

            mesh.SetNormals(normals);
            mesh.SetTangents(tangents);
            mesh.SetBinormals(binormals);
        }

        public static void Transform(Mesh mesh, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            var vertices = mesh.GetVertices();
            var normals = mesh.GetNormals();
            var tangents = mesh.GetTangents();
            var binormals = mesh.GetBinormals();
            var matrix = Matrix4x4.PRS(position, Quaternion.Euler(rotation.x, rotation.y, rotation.z),scale);

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = matrix * vertices[i];
                if (normals.Length > 0) normals[i] = (matrix * new Vector4(normals[i], 0)).xyz;
                if (tangents.Length > 0) tangents[i] = (matrix * new Vector4(tangents[i], 0)).xyz;
                if (binormals.Length > 0) binormals[i] = (matrix * new Vector4(binormals[i], 0)).xyz;
            }

            mesh.SetVertices(vertices);
            if (normals.Length > 0) mesh.SetNormals(normals);
            if (tangents.Length > 0) mesh.SetTangents(tangents);
            if (binormals.Length > 0) mesh.SetBinormals(binormals);
        }

        public static void ScaleVertex(Mesh mesh, Vector3 scale)
        {
            var vertices = mesh.GetVertices();

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(vertices[i].x * scale.x, vertices[i].y * scale.y, vertices[i].z * scale.z);
            }

            mesh.SetVertices(vertices);
        }

        public static void MergeMeshes(Mesh mesh, List<Mesh> meshes)
        {
            if (meshes.Count == 0) return;
            if (meshes.Count == 1)
            {
                mesh = meshes[0].Clone();
                return;
            }

            // Check if meshes are valid for merge (if they have the same components)
            int n_vertex = meshes[0].GetNumVertices();
            int n_normals = meshes[0].GetNumNormals();
            int n_uvs = meshes[0].GetNumUV();
            int n_color0 = meshes[0].GetNumColor0();
            int n_color1 = meshes[0].GetNumColor1();
            int n_binormal = meshes[0].GetNumBinormals();
            int n_tangent = meshes[0].GetNumTangents();

            for (int i = 1; i < meshes.Count; i++)
            {
                if (((n_vertex > 0) && (meshes[i].GetNumVertices() == 0)) ||
                    ((n_vertex == 0) && (meshes[i].GetNumVertices() > 0)))
                {
                    Debug.Log("Meshes can't be merged (one has positions, the other doesn't)");
                    return;
                }
                if (((n_normals > 0) && (meshes[i].GetNumNormals() == 0)) ||
                    ((n_normals == 0) && (meshes[i].GetNumNormals() > 0)))
                {
                    Debug.Log("Meshes can't be merged (one has normals, the other doesn't)");
                    return;
                }
                if (((n_uvs > 0) && (meshes[i].GetNumUV() == 0)) ||
                    ((n_uvs == 0) && (meshes[i].GetNumUV() > 0)))
                {
                    Debug.Log("Meshes can't be merged (one has UVs, the other doesn't)");
                    return;
                }
                if (((n_color0 > 0) && (meshes[i].GetNumColor0() == 0)) ||
                    ((n_color0 == 0) && (meshes[i].GetNumColor0() > 0)))
                {
                    Debug.Log("Meshes can't be merged (one has color0, the other doesn't)");
                    return;
                }
                if (((n_color1 > 0) && (meshes[i].GetNumColor1() == 0)) ||
                    ((n_color1 == 0) && (meshes[i].GetNumColor1() > 0)))
                {
                    Debug.Log("Meshes can't be merged (one has color1, the other doesn't)");
                    return;
                }
                if (((n_binormal > 0) && (meshes[i].GetNumBinormals() == 0)) ||
                    ((n_binormal == 0) && (meshes[i].GetNumBinormals() > 0)))
                {
                    Debug.Log("Meshes can't be merged (one has binormals, the other doesn't)");
                    return;
                }
                if (((n_tangent > 0) && (meshes[i].GetNumTangents() == 0)) ||
                    ((n_tangent == 0) && (meshes[i].GetNumTangents() > 0)))
                {
                    Debug.Log("Meshes can't be merged (one has tangents, the other doesn't)");
                    return;
                }
            }

            var vertices = new List<Vector3>(meshes[0].GetVertices());
            var normals = new List<Vector3>(meshes[0].GetNormals());
            var uvs = new List<Vector2>(meshes[0].GetUV());
            var color0 = new List<Color>(meshes[0].GetColor0());
            var color1 = new List<Color>(meshes[0].GetColor1());
            var tangents = new List<Vector3>(meshes[0].GetTangents());
            var binormals = new List<Vector3>(meshes[0].GetBinormals());
            var indices = new List<int>(meshes[0].GetTriangles());

            for (int i = 1; i < meshes.Count; i++)
            {
                int base_index = vertices.Count;
                if (vertices.Count > 0) { var v = meshes[i].GetVertices(); vertices.AddRange(v); }
                if (normals.Count > 0) { var v = meshes[i].GetNormals(); normals.AddRange(v); }
                if (uvs.Count > 0) { var v = meshes[i].GetUV(); uvs.AddRange(v); }
                if (color0.Count > 0) { var v = meshes[i].GetColor0(); color0.AddRange(v); }
                if (color1.Count > 0) { var v = meshes[i].GetColor1(); color1.AddRange(v); }
                if (tangents.Count > 0) { var v = meshes[i].GetTangents(); tangents.AddRange(v); }
                if (binormals.Count > 0) { var v = meshes[i].GetBinormals(); binormals.AddRange(v); }

                var src_index = meshes[i].GetTriangles();
                foreach (var j in src_index)
                {
                    indices.Add(j + base_index);
                }
            }

            mesh.Clear();
            if (vertices.Count > 0) mesh.SetVertices(vertices);
            if (normals.Count > 0) mesh.SetNormals(normals);
            if (uvs.Count > 0) mesh.SetUV(uvs);
            if (color0.Count > 0) mesh.SetColor0(color0);
            if (color1.Count > 0) mesh.SetColor1(color1);
            if (tangents.Count > 0) mesh.SetTangents(tangents);
            if (binormals.Count > 0) mesh.SetBinormals(binormals);
            if (indices.Count > 0) mesh.SetTriangles(indices);
        }

        public static void SetColor0(Mesh mesh, Color color)
        {
            List<Color> colors = new List<Color>();

            for (int i = 0; i < mesh.GetNumVertices(); i++)
            {
                colors.Add(color);
            }

            mesh.SetColor0(colors);
        }

        public static void SetColor1(Mesh mesh, Color color)
        {
            List<Color> colors = new List<Color>();

            for (int i = 0; i < mesh.GetNumVertices(); i++)
            {
                colors.Add(color);
            }

            mesh.SetColor1(colors);
        }


        public static void InvertV(Mesh mesh)
        {
            var uv = mesh.GetUV();

            for (int i = 0; i < uv.Length; i++)
            {
                uv[i] = new Vector2(uv[i].x, 1.0f - uv[i].y);
            }

            mesh.SetUV(uv);
        }

        public static void CopyNormalsToColor0(Mesh mesh)
        {
            var normals = mesh.GetNormals();
            var color = new Color[normals.Length];
            
            for (int i = 0; i < normals.Length; i++)
            {
                color[i] = (Color)(normals[i] * 0.5f + 0.5f);
            }

            mesh.colors0 = color;
        }
    }
}
