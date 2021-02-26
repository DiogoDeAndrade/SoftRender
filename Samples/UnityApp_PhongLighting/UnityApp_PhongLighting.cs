using SoftRender.Engine;
using SoftRender.UnityApp;
using SoftRender.UnityApp.Defaults;
using Mathlib;

namespace SoftRender.Samples.UnityApp.PhongLighting
{
    class PhongLighting : SoftRender.UnityApp.Application
    {
        GameObject lightObject;

        public PhongLighting()
        {
            name = "Phong Lighting Sample - Unity Framework";
            enableDepthBuffer = true;
            windowResX = 1280;
            windowResY = 960;
            resScale = 0.25f;
            writeFPS = (resScale * windowResX) >= 320;
        }

        protected override bool Initialize()
        {
            if (!base.Initialize()) return false;

            var mesh = GeometricFactory.BuildPlane("Plane", 16.0f, 32, Vector2.one, false);

            var meshObject = new GameObject("Object");
            var meshFilter = meshObject.AddComponent<MeshFilter>();
            var meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material
            {
                isWireframe = false,
                baseColor = Color.white,
                specPower = 5.0f,
                specIntensity = 1.0f,
                shader = new Shaders.PhongLighting()
            };
            meshFilter.mesh = mesh;

            var fpCameraMove = mainCamera.gameObject.AddComponent<FPCameraMove>();
            fpCameraMove.displayCoords = false;
            mainCamera.transform.position = new Vector3(0.0f, 1.5f, -10.0f);
            mainCamera.pixelPerfect = true;
            mainCamera.orthographic = false;
            mainCamera.farClipPlane = 1000.0f;

            // Create a light
            lightObject = new GameObject("Light");
            lightObject.transform.position = new Vector3(4.0f, 2.0f, 0.0f);
            var light = lightObject.AddComponent<Light>();
            light.intensity = 0.5f;
            light.color = Color.red;
            var rotationComponent = lightObject.AddComponent<RotateAroundXZ>();
            rotationComponent.centerPoint = new Vector3(0, 2, 0);

            return true;
        }

        protected override void Loop()
        {
            base.Loop();

            if ((resScale * windowResX) > 320)
            {
                string loopTimeText = string.Format("Loop Time = {0,7:###.000}s", loopTime);
                Write(0, 40, loopTimeText, Color.yellow, Color.black);
            }
        }
    }
}
