using SoftRender.Engine;
using System.Collections.Generic;

namespace SoftRender.UnityApp
{
    public class Mesh : Object
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
        bool        anythingButPosition;

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
                anythingButPosition = true;
            }
        }
        public Color[] colors1
        {
            get => _colors1;
            set
            {
                _colors1 = value;
                vertex_modified = true;
                anythingButPosition = true;
            }
        }

        FatVertex[] vertexStream;

        public Mesh(string name) : base(name)
        {
            vertex_modified = true;
            anythingButPosition = false;
            _uv = new Vector2[8][];
        }

        public void Clear()
        {
            _vertices = null;
            _normals = null;
            _colors0 = null;
            _colors1 = null;
            _uv = new Vector2[8][];
            _triangles = null;
            vertex_modified = true;
            anythingButPosition = true;
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

        public void Render(Matrix4x4 objectClipMatrix, Material material)
        {
            if (vertex_modified)
            {
                // Prepare vertex stream
                if ((vertexStream == null) || (vertexStream.Length != vertices.Length))
                {
                    vertexStream = new FatVertex[vertices.Length];
                }

                if (colors0 != null) for (int i = 0; i < vertices.Length; i++) vertexStream[i].color = _colors0[i];
            }

            int w = Application.currentScreen.width >> 1;
            int h = Application.currentScreen.height >> 1;

            for (int i = 0; i < vertexStream.Length; i++)
            {
                vertexStream[i].position = objectClipMatrix * new Vector4(_vertices[i], 1);
                vertexStream[i].position.xyz /= vertexStream[i].position.w;
                vertexStream[i].position.x = vertexStream[i].position.x * w + w;
                vertexStream[i].position.y = -vertexStream[i].position.y * h + h;
            }

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
                if (anythingButPosition)
                {
                    var shouldCull = material.GetCullFunction();

                    for (int i = 0; i < _triangles.Length; i+=3)
                    {
                        if (!shouldCull(vertexStream[_triangles[i]], vertexStream[_triangles[i + 1]], vertexStream[_triangles[i + 2]]))
                        {
                            Application.currentScreen.DrawTriangle(vertexStream[_triangles[i]],
                                                                   vertexStream[_triangles[i + 1]],
                                                                   vertexStream[_triangles[i + 2]]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < _triangles.Length; i = i + 3)
                    {
                        Application.currentScreen.DrawTriangle(vertexStream[_triangles[i]].position.xy,
                                                               vertexStream[_triangles[i + 1]].position.xy,
                                                               vertexStream[_triangles[i + 2]].position.xy,
                                                               material.baseColor);
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
            mesh.anythingButPosition = anythingButPosition;

            return mesh;
        }

        static Vector3 GetFaceNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            return Vector3.Cross(v2 - v1, v3 - v1);
        }
    }
}
