using System.Collections.Generic;
using Mathlib;

namespace SoftRender.UnityApp
{
    public enum CameraClearFlags { SolidColor, Depth, Nothing };

    public class Camera : Behaviour
    {
        public CameraClearFlags clearFlags = CameraClearFlags.SolidColor;
        public Color            backgroundColor = Color.unityBlue;
        public int              depth = 0;
        public uint             cullingMask = 0xFFFFFFFF;
        public bool             orthographic = true;
        public float            orthographicSize = 480;
        public float            nearClipPlane = 0.3f;
        public float            farClipPlane = 1000.0f;
        public float            fieldOfView = 60.0f;
        public bool             pixelPerfect = false;
        public bool             sortObjects = true;

        int viewportWidth { get { return Application.current.resX; } }
        int viewportHeight { get { return Application.current.resY; } }

        public static Camera       current = null;

        static List<Camera> cameras = new List<Camera>();

        public static List<Camera> allCameras
        {
            get
            {
                List<Camera> ret = new List<Camera>();
                foreach (var camera in cameras)
                {
                    if (camera.isActiveAndEnabled) ret.Add(camera);
                }
                return ret;
            }
        }
        public static int allCamerasCount
        {
            get
            {
                int count = 0;
                foreach (var camera in cameras)
                {
                    if (camera.isActiveAndEnabled) count++;
                }
                return count;
            }
        }

        public Camera() : base()
        {
            // Register the camera in the system
            cameras.Add(this);
        }

        ~Camera()
        {
            cameras.Remove(this);
        }

        public Matrix4x4 GetProjectionMatrix()
        {
            if (orthographic)
            {
                return Matrix4x4.Ortographic(viewportWidth, viewportHeight, nearClipPlane, farClipPlane);
            }
            else
            {
                return Matrix4x4.Perspective(fieldOfView, viewportWidth, viewportHeight, nearClipPlane, farClipPlane);
            }
        }

        public Matrix4x4 GetClipMatrix()
        {
            var proj = GetProjectionMatrix();
            var pos = (pixelPerfect && orthographic) ? (Vector3.Round(gameObject.transform.position)) : (gameObject.transform.position);
            var prs = Matrix4x4.PR(pos, gameObject.transform.rotation).inverse;

            return prs * proj;
        }

        public void Render(List<Renderer> renderables)
        {
            switch (clearFlags)
            {
                case CameraClearFlags.SolidColor:
                    Application.currentScreen.Clear(backgroundColor, float.MaxValue);
                    break;
                case CameraClearFlags.Depth:
                    Application.currentScreen.Clear(float.MaxValue);
                    break;
                case CameraClearFlags.Nothing:
                    break;
                default:
                    break;
            }

            current = this;

            var cameraPos = transform.position;

            renderables.Sort((r1, r2) => Vector3.Distance(cameraPos, r1.transform.position).CompareTo(Vector3.Distance(cameraPos, r2.transform.position)));

            foreach (var renderable in renderables)
            {
                Renderer.current = renderable;
                renderable.Render();
            }

            Renderer.current = null;
            current = null;
        }
    }
}
