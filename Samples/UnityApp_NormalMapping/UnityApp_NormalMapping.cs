using SoftRender.Engine;
using SoftRender.UnityApp;
using SoftRender.Shaders;
using SoftRender.UnityApp.Defaults;
using SoftRender.UnityApp.SceneManagement;

namespace SoftRender.Samples.UnityApp.NormalMapping
{
    class NormalMapping : SoftRender.UnityApp.Application
    {
        public NormalMapping()
        {
            name = "Normal Mapping Sample - Unity Framework";
            enableDepthBuffer = true;
            windowResX = 1280;
            windowResY = 960;
            resScale = 0.125f;
            writeFPS = (resScale * windowResX) >= 320;
        }

        protected override bool Initialize()
        {
            if (!base.Initialize()) return false;

            {
                // Ground setup
                var groundPlane = GeometricFactory.BuildPlane("Plane", 64.0f, 32, Vector2.one, true);

                var meshObject = new GameObject("Object");
                var meshFilter = meshObject.AddComponent<MeshFilter>();
                var meshRenderer = meshObject.AddComponent<MeshRenderer>();
                meshRenderer.material = new Material
                {
                    isWireframe = false,
                    baseColor = new Color(0.2f, 0.6f, 0.2f, 1.0f),
                    specPower = 5.0f,
                    specIntensity = 0.0f,
                    albedo = Resources.Load<Texture>("rock_albedo.png"),
                    normal = Resources.Load<Texture>("rock_normal.png"),
                    shader = new Shaders.NormalMapping()
                };
                meshFilter.mesh = groundPlane;
            }

            var fpCameraMove = mainCamera.gameObject.AddComponent<FPCameraMove>();
            fpCameraMove.displayCoords = false;
            mainCamera.transform.position = new Vector3(0.0f, 1.5f, -10.0f);
            mainCamera.pixelPerfect = true;
            mainCamera.orthographic = false;
            mainCamera.farClipPlane = 1000.0f;

            // Create a light
            var lightObject = new GameObject("Light");
            lightObject.transform.position = new Vector3(4.0f, 2.0f, 0.0f);
            lightObject.AddComponent<Light>();
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
