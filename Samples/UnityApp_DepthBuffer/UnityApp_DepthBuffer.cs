using SoftRender.Engine;
using SoftRender.UnityApp;
using SoftRender.UnityApp.Defaults;
using Mathlib;

namespace SoftRender.Samples.UnityApp.DepthBuffer
{
    class DepthBuffer : SoftRender.UnityApp.Application
    {
        public DepthBuffer()
        {
            name = "Depth Buffer Sample - Unity Framework";
            writeFPS = true;
            enableDepthBuffer = true;
        }

        protected override bool Initialize()
        {
            if (!base.Initialize()) return false;

            var mesh = GeometricFactory.BuildCube("Cube", new Vector3(2.0f, 2.0f, 2.0f), false);
            MeshTools.CopyNormalsToColor0(mesh);

            int nCubes = 10;

            for (int i = 0; i < nCubes; i++)
            {
                var meshObject = new GameObject("Object");
                meshObject.transform.position = new Vector3(Random.Range(-20.0f, 20.0f), 1.0f, Random.Range(-20.0f, 20.0f));
                var meshFilter = meshObject.AddComponent<MeshFilter>();
                var meshRenderer = meshObject.AddComponent<MeshRenderer>();
                meshRenderer.material = new Material
                {
                    isWireframe = false,
                    baseColor = Color.yellow,
                    shader = new Shaders.VertexColor()
                };
                meshFilter.mesh = mesh;
            }

            mainCamera.gameObject.AddComponent<FPCameraMove>();
            mainCamera.transform.position = new Vector3(0.0f, 1.0f, 0.0f);
            mainCamera.pixelPerfect = true;
            mainCamera.orthographic = false;
            mainCamera.farClipPlane = 20.0f;

            return true;
        }
    }
}
