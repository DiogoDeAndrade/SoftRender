using System.Collections.Generic;
using SoftRender.Engine;
using SoftRender.UnityApp.SceneManagement;
using Mathlib;

namespace SoftRender.UnityApp
{
    public class Application : Engine.Application
    {
        static public Bitmap        currentScreen
        {
            get
            {
                return Application.current.GetScreen();
            }
        }
        public Camera mainCamera;

        public Application() : base()
        {
            SceneManager.CreateScene("StartupScene");

            var cameraObject = new GameObject("CameraObject");
            mainCamera = cameraObject.AddComponent<Camera>();

            (windowResX, windowResY) = (1280, 960);
            resScale = 1.0f;
        }

        ~Application()
        {
            current = null;
        }

        protected override bool Startup()
        {
            if (!base.Startup()) return false;

            return true;
        }

        protected override void Loop()
        {
            // Poll keyboard
            Input.PollKeyboard();

            // Run update method
            MonoBehaviour.RunUpdate();

            // Render all scenes to all cameras
            var cameras = Camera.allCameras;
            cameras.Sort((c1, c2) => c1.depth.CompareTo(c2.depth));

            foreach (var camera in cameras)
            {
                // Filter renderables
                var toRender = new List<Renderer>();
                foreach (var renderer in Renderer.allRenderables)
                {
                    if (renderer.isActiveAndEnabled)
                    {
                        if ((camera.cullingMask & (1 << renderer.gameObject.layer)) != 0)
                        {
                            toRender.Add(renderer);
                        }
                    }
                }
                camera.Render(toRender);
            }

            // Run post render method
            MonoBehaviour.RunOnPostUpdate();
        }

        public static void Write(int x, int y, string txt, Color textColor, Color backgroundColor)
        {
            currentScreen.Write(x, y, txt, current.GetDefaultFont(), textColor, backgroundColor);
        }
    }
}
