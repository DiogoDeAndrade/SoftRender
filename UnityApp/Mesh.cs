using SoftRender.Engine;

namespace SoftRender.UnityApp
{
    public class Mesh : Object
    {
        public int[]       triangles;
        public Vector3[]   vertices;

        Vector4[]   transformedVertices;

        public Mesh(string name) : base(name)
        {

        }

        public void Render(Matrix4x4 objectClipMatrix)
        {
            if ((transformedVertices == null) || (transformedVertices.Length != vertices.Length))
            {
                transformedVertices = new Vector4[vertices.Length];
            }

            int w = Application.currentScreen.width >> 1;
            int h = Application.currentScreen.height >> 1;

            for (int i = 0; i < transformedVertices.Length; i++)
            {
                transformedVertices[i] = objectClipMatrix * new Vector4(vertices[i], 1);
                transformedVertices[i] = transformedVertices[i] / transformedVertices[i].w;
                transformedVertices[i].x = transformedVertices[i].x * w + w;
                transformedVertices[i].y = -transformedVertices[i].y * h + h;
            }

            for (int i = 0; i < triangles.Length; i+=3)
            {
                Application.currentScreen.DrawTriangle(transformedVertices[triangles[i]].xy,
                                                       transformedVertices[triangles[i + 1]].xy,
                                                       transformedVertices[triangles[i + 2]].xy,
                                                       Color32.magenta);
            }
        }
    }
}
