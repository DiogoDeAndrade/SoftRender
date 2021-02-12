using SoftRender.Engine;
using SoftRender.UnityApp;
using SoftRender.Shaders;
using SoftRender.UnityApp.Defaults;
using SoftRender.UnityApp.SceneManagement;

namespace SoftRender.Samples.UnityApp.Fog
{
    class Fog : SoftRender.UnityApp.Application
    {
        public Fog()
        {
            name = "Fog Sample - Unity Framework";
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
                var groundPlane = GeometricFactory.BuildPlane("Plane", 64.0f, 32, Vector2.one, false);

                var meshObject = new GameObject("Object");
                var meshFilter = meshObject.AddComponent<MeshFilter>();
                var meshRenderer = meshObject.AddComponent<MeshRenderer>();
                meshRenderer.material = new Material
                {
                    isWireframe = false,
                    baseColor = new Color(0.2f, 0.8f, 0.2f, 1.0f),
                    specPower = 5.0f,
                    specIntensity = 0.0f,
                    shader = new Shaders.PhongLightingWithFog()
                };
                meshFilter.mesh = groundPlane;
            }

            {
                // Forest
                var tree = Resources.Load<Mesh>("pinetree.obj");

                for (int i = 0; i < 10; i++)
                {
                    var meshObject = new GameObject("Object");
                    meshObject.transform.position = new Vector3(Random.Range(-25.0f, 25.0f), 0.0f, Random.Range(-25.0f, 25.0f));
                    meshObject.transform.rotation = Quaternion.Euler(0.0f, Random.Range(0, 360), 0.0f);
                    var meshFilter = meshObject.AddComponent<MeshFilter>();
                    var meshRenderer = meshObject.AddComponent<MeshRenderer>();
                    meshRenderer.material = new Material
                    {
                        isWireframe = false,
                        baseColor = Color.white,
                        shader = new Shaders.PhongLightingWithFog()
                    };
                    meshFilter.mesh = tree;
                }
            }

            var fpCameraMove = mainCamera.gameObject.AddComponent<FPCameraMove>();
            fpCameraMove.displayCoords = false;
            mainCamera.transform.position = new Vector3(0.0f, 1.5f, -10.0f);
            mainCamera.pixelPerfect = true;
            mainCamera.orthographic = false;
            mainCamera.farClipPlane = 1000.0f;
            mainCamera.backgroundColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);

            var scene = SceneManager.GetActiveScene();
            scene.ambientLight = new Color(0.1f, 0.1f, 0.1f, 1.0f);
            scene.fogColor = mainCamera.backgroundColor;
            scene.fogEnd = 30.0f;

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
