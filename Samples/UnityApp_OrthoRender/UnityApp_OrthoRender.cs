using SoftRender.Engine;
using SoftRender.UnityApp;

namespace SoftRender.Samples.UnityApp.OrthoRender
{
    class OrthoRender : SoftRender.UnityApp.Application
    {
        public OrthoRender()
        {
            name = "Ortho Render Sample - Unity Framework";
        }

        protected override bool Initialize()
        {
            if (!base.Initialize()) return false;

            var mesh = new Mesh("Triangle");
            mesh.vertices = new Vector3[] { new Vector3(-200, -200, 1), new Vector3(200, 0, 1), new Vector3(0, 200, 1) };
            mesh.triangles = new int[] { 0, 1, 2 };

            var meshObject = new GameObject("MeshObject");
            var meshFilter = meshObject.AddComponent<MeshFilter>();
            var meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter.mesh = mesh;

            mainCamera.gameObject.AddComponent<CameraMove>();
            mainCamera.pixelPerfect = true;

            return true;
        }
    }
}
