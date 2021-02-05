using SoftRender.Engine;
using SoftRender.UnityApp;

namespace SoftRender.Samples.UnityApp.PerspectiveRender
{
    class PerspectiveRender : SoftRender.UnityApp.Application
    {
        public PerspectiveRender()
        {
            name = "Perspective Render Sample - Unity Framework";
        }

        protected override bool Initialize()
        {
            if (!base.Initialize()) return false;

            //var mesh = GeometricFactory.BuildCube("Cube", new Vector3(2.0f, 2.0f, 2.0f), false);
            var mesh = GeometricFactory.BuildCone("Cone", new Vector3(1.0f, 2.0f, 1.0f), 0.0f, 16, false);
            MeshTools.CopyNormalsToColor0(mesh);
            
            var meshObject = new GameObject("Object");
            meshObject.transform.position = new Vector3(0, 0, 5);
            var meshFilter = meshObject.AddComponent<MeshFilter>();
            var meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material
            {
                isWireframe = false,
                baseColor = Color.yellow
            };
            meshFilter.mesh = mesh;

            meshObject = new GameObject("WireObject");
            meshObject.transform.position = new Vector3(0, 0, 5);
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material
            {
                isWireframe = true,
                baseColor = Color.black
            };
            meshFilter.mesh = mesh;

            mainCamera.gameObject.AddComponent<FPCameraMove>();
            mainCamera.transform.position = new Vector3(0.0f, 1.0f, 0.0f);
            //mainCamera.transform.rotation = Quaternion.Euler(24.0f, 84.0f, 0);
            mainCamera.pixelPerfect = true;
            mainCamera.orthographic = false;

            return true;
        }
    }
}
