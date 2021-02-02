using SoftRender.Engine;

namespace SoftRender.UnityApp
{
    public class MeshRenderer : Renderer
    {
        override public void Render(Matrix4x4 cameraClipMatrix)
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null) return;

            Mesh mesh = meshFilter.mesh;
            if (mesh == null) return;

            var objectClipMatrix = transform.localToWorldMatrix * cameraClipMatrix;
            
            mesh.Render(objectClipMatrix);
        }
    }
}
