using SoftRender.Engine;
using SoftRender.UnityApp;
using SoftRender.Shaders;
using SoftRender.UnityApp.Defaults;
using SoftRender.UnityApp.SceneManagement;

namespace SoftRender.Samples.UnityApp.Textures
{
    class Textures : SoftRender.UnityApp.Application
    {
        public Textures()
        {
            name = "Texturing Sample - Unity Framework";
            enableDepthBuffer = true;
            windowResX = 1280;
            windowResY = 960;
            resScale = 0.5f; // 0.125f;
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
                    baseColor = new Color(0.2f, 0.6f, 0.2f, 1.0f),
                    specPower = 5.0f,
                    specIntensity = 0.0f,
                    shader = new Shaders.MaterialColor()
                };
                meshFilter.mesh = groundPlane;
            }

            {
                var cubeTexture = Resources.Load<Texture>("cube_texture.png");

                var meshObject = new GameObject("Object");
                meshObject.transform.position = new Vector3(0, 2, 0);
                var meshFilter = meshObject.AddComponent<MeshFilter>();
                var meshRenderer = meshObject.AddComponent<MeshRenderer>();
                meshRenderer.material = new Material
                {
                    albedo = cubeTexture,
                    shader = new Shaders.AlbedoTexture()
                };
                meshFilter.mesh = GeometricFactory.BuildCube("Cube", Vector3.one * 4.0f, true);
            }//*/

            var fpCameraMove = mainCamera.gameObject.AddComponent<FPCameraMove>();
            fpCameraMove.displayCoords = false;
            mainCamera.transform.position = new Vector3(0.0f, 1.5f, -10.0f);
            mainCamera.pixelPerfect = true;
            mainCamera.orthographic = false;
            mainCamera.farClipPlane = 1000.0f;

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
