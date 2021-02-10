using SoftRender.Engine;
using SoftRender.UnityApp;
using SoftRender.Shaders;
using SoftRender.UnityApp.Defaults;

namespace SoftRender.Samples.UnityApp.PixelLighting
{
    class PixelLighting : SoftRender.UnityApp.Application
    {
        GameObject lightObject;

        public PixelLighting()
        {
            name = "Pixel Lighting Sample - Unity Framework";
            writeFPS = true;
            enableDepthBuffer = true;
            windowResX = 640;
            windowResY = 480;
        }

        protected override bool Initialize()
        {
            if (!base.Initialize()) return false;

            string model = "ship";

            var mesh = Resources.Load<Mesh>(model + ".obj");

            var meshObject = new GameObject("Object");
            var meshFilter = meshObject.AddComponent<MeshFilter>();
            var meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material
            {
                isWireframe = false,
                baseColor = Color.yellow,
                shader = new Shaders.PixeLLighting()
            };
            meshFilter.mesh = mesh;

            mainCamera.gameObject.AddComponent<FPCameraMove>();
            mainCamera.transform.position = new Vector3(0.0f, 1.0f, -10.0f);
            mainCamera.pixelPerfect = true;
            mainCamera.orthographic = false;
            mainCamera.farClipPlane = 1000.0f;

            if (model == "castle")
            {
                mainCamera.transform.position = new Vector3(0.0f, 10.0f, -30.0f);
            }

            // Create a light
            lightObject = new GameObject("Light");
            lightObject.transform.position = new Vector3(4.0f, 2.0f, 0.0f);
            lightObject.AddComponent<Light>();
            var rotationComponent = lightObject.AddComponent<RotateAroundXZ>();
            rotationComponent.centerPoint = new Vector3(0, 2, 0);

            return true;
        }

        protected override void Loop()
        {
            base.Loop();

            string loopTimeText = string.Format("Loop Time = {0,7:###.000}s", loopTime);
            Write(0, 40, loopTimeText, Color.yellow, Color.black);
        }
    }
}
