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

            var meshObject = new GameObject("MeshObjectSolid");
            meshObject.transform.position += Vector3.left * 150.0f;
            var meshFilter = meshObject.AddComponent<MeshFilter>();
            var meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material();
            meshRenderer.material.baseColor = Color.yellow;
            meshFilter.mesh = mesh;

            meshObject = new GameObject("MeshObjectWireframe");
            meshObject.transform.position += Vector3.left * 450.0f;
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material();
            meshRenderer.material.isWireframe = true;
            meshRenderer.material.baseColor = Color.green;
            meshFilter.mesh = mesh;

            mesh = new Mesh("TriangleVertexColor");
            mesh.vertices = new Vector3[] { new Vector3(-200, -200, 1), new Vector3(200, 0, 1), new Vector3(0, 200, 1) };
            mesh.colors = new Color[] { new Color(1, 0, 0, 1), new Color(1, 1, 0, 1), new Color(0, 1, 0, 1) };
            mesh.triangles = new int[] { 0, 1, 2 };

            meshObject = new GameObject("MeshObject");
            meshObject.transform.position += Vector3.right * 150.0f;
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material();
            meshRenderer.material.baseColor = Color.red;
            meshFilter.mesh = mesh;

            mainCamera.gameObject.AddComponent<CameraMove>();
            mainCamera.pixelPerfect = true;

            return true;
        }
    }
}
