using SoftRender.Engine;

namespace SoftRender.UnityApp
{
    public class Mesh : Object
    {
        int[]       _triangles;
        Vector3[]   _vertices;
        Color[]     _colors;
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
        public Color[] colors
        {
            get => _colors;
            set
            {
                _colors = value;
                vertex_modified = true;
                anythingButPosition = true;
            }
        }

        FatVertex[] vertexStream;

        public Mesh(string name) : base(name)
        {
            vertex_modified = true;
            anythingButPosition = false;
        }

        public void Render(Matrix4x4 objectClipMatrix, Material material)
        {
            if (vertex_modified)
            {
                // Prepare vertex stream
                if ((vertexStream == null) || (vertexStream.Length != vertices.Length))
                {
                    vertexStream = new FatVertex[vertices.Length];
                }

                if (colors != null) for (int i = 0; i < vertices.Length; i++) vertexStream[i].color = _colors[i];
            }

            int w = Application.currentScreen.width >> 1;
            int h = Application.currentScreen.height >> 1;

            for (int i = 0; i < vertexStream.Length; i++)
            {
                vertexStream[i].position = objectClipMatrix * new Vector4(_vertices[i], 1);
                vertexStream[i].position /= vertexStream[i].position.w;
                vertexStream[i].position.x = vertexStream[i].position.x * w + w;
                vertexStream[i].position.y = -vertexStream[i].position.y * h + h;
            }

            if (material.isWireframe)
            {
                for (int i = 0; i < _triangles.Length; i += 3)
                {
                    Application.currentScreen.DrawLine(vertexStream[_triangles[i]].position.xy,
                                                       vertexStream[_triangles[i + 1]].position.xy, material.baseColor);
                    Application.currentScreen.DrawLine(vertexStream[_triangles[i + 1]].position.xy,
                                                       vertexStream[_triangles[i + 2]].position.xy, material.baseColor);
                    Application.currentScreen.DrawLine(vertexStream[_triangles[i + 2]].position.xy,
                                                       vertexStream[_triangles[i]].position.xy, material.baseColor);
                }
            }
            else
            {
                if (anythingButPosition)
                {
                    for (int i = 0; i < _triangles.Length; i += 3)
                    {
                        Application.currentScreen.DrawTriangle(vertexStream[_triangles[i]],
                                                               vertexStream[_triangles[i + 1]],
                                                               vertexStream[_triangles[i + 2]]);
                    }
                }
                else
                {
                    for (int i = 0; i < _triangles.Length; i += 3)
                    {
                        Application.currentScreen.DrawTriangle(vertexStream[_triangles[i]].position.xy,
                                                               vertexStream[_triangles[i + 1]].position.xy,
                                                               vertexStream[_triangles[i + 2]].position.xy,
                                                               material.baseColor);
                    }
                }
            }
        }
    }
}
