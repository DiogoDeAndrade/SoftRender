using SoftRender.Engine;

namespace SoftRender.UnityApp
{
    public class MeshRenderer : Renderer
    {
        public Material material;

        override public void Render()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null) return;

            Mesh mesh = meshFilter.mesh;
            if (mesh == null) return;

            if (material == null) return;

            mesh.Render(material);
        }
    }
}
