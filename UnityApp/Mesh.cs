﻿#define PARALELL_VERTEX_PROGRAM

using SoftRender.Engine;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mathlib;

namespace SoftRender.UnityApp
{

    public class Mesh : Resource
    {
        int[]       _triangles;
        Vector3[]   _vertices;
        Vector3[]   _normals;
        Vector3[]   _tangents;
        Vector3[]   _binormals;
        Color[]     _colors0;
        Color[]     _colors1;
        Vector2[][] _uv;
        bool        vertex_modified;

        public Material defaultMaterial;

        public int[] triangles
        {
            get => _triangles;
            set => _triangles = value;
        }
        public Vector3[] vertices
        {
            get => _vertices;
            set
            {
                _vertices = value;
                vertex_modified = true;
            }
        }
        public Color[] colors0
        {
            get => _colors0;
            set
            {
                _colors0 = value;
                vertex_modified = true;
            }
        }
        public Color[] colors1
        {
            get => _colors1;
            set
            {
                _colors1 = value;
                vertex_modified = true;
            }
        }

        FatVertex[] sourceStream;
        FatVertex[] vertexStream;

        public Mesh(string name) : base(name)
        {
            defaultMaterial = null;
            vertex_modified = true;
            _uv = new Vector2[2][];
        }

        public void Clear()
        {
            _vertices = null;
            _normals = null;
            _colors0 = null;
            _colors1 = null;
            _uv = new Vector2[2][];
            _triangles = null;
            vertex_modified = true;
        }

        public int GetNumVertices() => (_vertices != null)?(_vertices.Length):(0);
        public void SetVertices(List<Vector3> vertices)
        {
            _vertices = vertices.ToArray();
            vertex_modified = true;
        }
        public void SetVertices(Vector3[] vertices)
        {
            _vertices = vertices;
            vertex_modified = true;
        }
        public Vector3[] GetVertices() => _vertices;

        public int GetNumNormals() => (_normals != null)?(_normals.Length):(0);
        public void SetNormals(Vector3[] normals)
        {
            _normals = normals;
            vertex_modified = true;
        }
        public void SetNormals(List<Vector3> normals)
        {
            _normals = normals.ToArray();
            vertex_modified = true;
        }
        public Vector3[] GetNormals() => _normals;
        public int GetNumTangents() => (_tangents != null)?(_tangents.Length):(0);
        public void SetTangents(Vector3[] tangents)
        {
            _tangents = tangents;
            vertex_modified = true;
        }
        public void SetTangents(List<Vector3> tangents)
        {
            _tangents = tangents.ToArray();
            vertex_modified = true;
        }
        public Vector3[] GetTangents() => _tangents;
        public int GetNumBinormals() => (_binormals != null)?(_binormals.Length):(0);
        public void SetBinormals(Vector3[] binormals) 
        {
            _binormals = binormals;
            vertex_modified = true;
        }
        public void SetBinormals(List<Vector3> binormals)
        {
            _binormals = binormals.ToArray();
            vertex_modified = true;
        }
        public Vector3[] GetBinormals() => _binormals;

        public int GetNumUV() => (_uv != null) ? ((_uv[0] != null) ? (_uv[0].Length) : (0)) : (0);
        public int GetNumUV(int channel) => (_uv != null) ? ((_uv[channel] != null) ? (_uv[channel].Length) : (0)) : (0);
        public void SetUV(List<Vector2> vertices) => SetUV(0, vertices);
        public void SetUV(Vector2[] vertices) => SetUV(0, vertices);
        public Vector2[] GetUV() => _uv[0];

        public void SetUV(int channel, List<Vector2> uv)
        {
            _uv[channel] = uv.ToArray();
            vertex_modified = true;
        }

        public void SetUV(int channel, Vector2[] uv)
        {
            _uv[channel] = uv;
            vertex_modified = true;
        }

        public Vector2[] GetUV(int channel) => _uv[channel];
        public int GetNumColor0() => (_colors0 != null)?(_colors0.Length):(0);
        public void SetColor0(Color[] colors)
        {
            _colors0 = colors;
            vertex_modified = true;
        }
        public void SetColor0(List<Color> colors)
        {
            _colors0 = colors.ToArray();
            vertex_modified = true;
        }
        public Color[] GetColor0() => _colors0;
        public int GetNumColor1() => (_colors1 != null) ? (_colors0.Length) : (0);
        public void SetColor1(Color[] colors)
        {
            _colors1 = colors;
            vertex_modified = true;
        }
        public void SetColor1(List<Color> colors)
        {
            _colors1 = colors.ToArray();
            vertex_modified = true;
        }
        public Color[] GetColor1() => _colors1;

        public void SetTriangles(List<int> indices)
        {
            _triangles = indices.ToArray();
        }
        public void SetTriangles(int[] indices)
        {
            _triangles = indices;
        }
        public int[] GetTriangles() => _triangles;

        static Shader defaultShader = null;

        public void Render(Material material)
        {
            if (vertex_modified)
            {
                // Prepare vertex stream
                if ((vertexStream == null) || (vertexStream.Length != vertices.Length))
                {
                    sourceStream = new FatVertex[_vertices.Length];

                    vertexStream = new FatVertex[_vertices.Length];
                }

                if (_vertices != null) for (int i = 0; i < vertices.Length; i++) sourceStream[i].position = new Vector4(_vertices[i], 1);
                if (_normals != null) for (int i = 0; i < vertices.Length; i++) sourceStream[i].normal = _normals[i];
                if (_tangents != null) for (int i = 0; i < vertices.Length; i++) sourceStream[i].tangent = _tangents[i];
                if (_binormals != null) for (int i = 0; i < vertices.Length; i++) sourceStream[i].binormal = _binormals[i];
                if (_colors0 != null) for (int i = 0; i < vertices.Length; i++) sourceStream[i].color0 = _colors0[i];
                if (_colors1 != null) for (int i = 0; i < vertices.Length; i++) sourceStream[i].color1 = _colors1[i];
                if (_uv[0] != null) for (int i = 0; i < vertices.Length; i++) sourceStream[i].uv0 = _uv[0][i];
                if (_uv[1] != null) for (int i = 0; i < vertices.Length; i++) sourceStream[i].uv1 = _uv[1][i];

                vertex_modified = false;
            }

            // Get the shader
            Shader      shader = material.shader;
            if (shader == null)
            {
                Debug.Log("No shader setup for material " + material.name);
                return;
            }

            shader.Setup(material);

            var vertexProgram = shader.GetVertexProgram();

            int halfWidth = Application.currentScreen.width >> 1;
            int halfHeight = Application.currentScreen.height >> 1;

#if PARALELL_VERTEX_PROGRAM
            const int clusterSize = 1024;

            int clusters = vertexStream.Length / clusterSize;
            var transformStage = Parallel.For(0, clusters, i =>
            {
                int idx = i * clusterSize;
                for (int j = 0; j < clusterSize; j++)
                {
                    vertexProgram(sourceStream[idx], ref vertexStream[idx]);
                    vertexStream[idx].position.x = (vertexStream[idx].position.x / vertexStream[idx].position.w) * halfWidth + halfWidth;
                    vertexStream[idx].position.y = (-vertexStream[idx].position.y / vertexStream[idx].position.w) * halfHeight + halfHeight;
                    vertexStream[idx].position.z = 1.0f / vertexStream[idx].position.z;
                    vertexStream[idx].MultiplyAttributes(vertexStream[idx].position.z);
                    //vertexStream[idx].position.z = (vertexStream[idx].position.z / vertexStream[idx].position.w);

                    idx++;
                }
            });

            // Do the remaining
            for (int idx = clusters * clusterSize; idx < vertexStream.Length; idx++)
            {
                vertexProgram(sourceStream[idx], ref vertexStream[idx]);
                vertexStream[idx].position.x = (vertexStream[idx].position.x / vertexStream[idx].position.w) * halfWidth + halfWidth;
                vertexStream[idx].position.y = (-vertexStream[idx].position.y / vertexStream[idx].position.w) * halfHeight + halfHeight;
                vertexStream[idx].position.z = 1.0f / vertexStream[idx].position.z;
                vertexStream[idx].MultiplyAttributes(vertexStream[idx].position.z);
                //vertexStream[idx].position.z = (vertexStream[idx].position.z / vertexStream[idx].position.w);
            }

            while (!transformStage.IsCompleted) { ; }

#else
            for (int i = 0; i < vertexStream.Length; i++)
            {
                vertexProgram(sourceStream[i], ref vertexStream[i]);
                vertexStream[i].position.x = (vertexStream[i].position.x / vertexStream[i].position.w) * halfWidth + halfWidth;
                vertexStream[i].position.y = (-vertexStream[i].position.y / vertexStream[i].position.w) * halfHeight + halfHeight;
                //vertexStream[i].position.z = (vertexStream[i].position.z / vertexStream[i].position.w);
            }
#endif

            if (material.isWireframe)
            {
                var shouldCull = material.GetCullFunction();

                for (int i = 0; i < _triangles.Length; i+=3)
                {
                    if (!shouldCull(vertexStream[_triangles[i]], vertexStream[_triangles[i + 1]], vertexStream[_triangles[i + 2]]))
                    {
                        Application.currentScreen.DrawLine(vertexStream[_triangles[i]].position.xy,
                                                       vertexStream[_triangles[i + 1]].position.xy, material.baseColor);
                        Application.currentScreen.DrawLine(vertexStream[_triangles[i + 1]].position.xy,
                                                           vertexStream[_triangles[i + 2]].position.xy, material.baseColor);
                        Application.currentScreen.DrawLine(vertexStream[_triangles[i + 2]].position.xy,
                                                           vertexStream[_triangles[i]].position.xy, material.baseColor);
                    }
                }
            }
            else
            {
                var shouldCull = material.GetCullFunction();

                for (int i = 0; i < _triangles.Length; i+=3)
                {
                    if (!shouldCull(vertexStream[_triangles[i]], vertexStream[_triangles[i + 1]], vertexStream[_triangles[i + 2]]))
                    {
                        Application.currentScreen.DrawTriangle(vertexStream[_triangles[i]],
                                                                vertexStream[_triangles[i + 1]],
                                                                vertexStream[_triangles[i + 2]],
                                                                material);
                    }
                }
            }
        }

        public Mesh Clone()
        {
            Mesh mesh = new Mesh("Clone of " + name);

            mesh._triangles = _triangles;
            mesh._vertices = _vertices;
            mesh._normals = _normals;
            mesh._tangents = _tangents;
            mesh._binormals = _binormals;
            mesh._colors0 = _colors0;
            mesh._colors1 = _colors1;
            mesh._uv = _uv;
            mesh.vertex_modified = true;

            return mesh;
        }

        static Vector3 GetFaceNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            return Vector3.Cross(v2 - v1, v3 - v1);
        }
    }
}
