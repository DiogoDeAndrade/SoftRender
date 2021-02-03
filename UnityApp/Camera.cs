using SoftRender.Engine;
using System.Collections.Generic;

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
        public bool             pixelPerfect = false;

        int viewportWidth { get { return Application.current.resX; } }
        int viewportHeight { get { return Application.current.resY; } }

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

        Matrix4x4 GetProjectionMatrix()
        {
            if (orthographic)
            {
                return Matrix4x4.Ortographic(viewportWidth, viewportHeight, nearClipPlane, farClipPlane);
            }
            else
            {
                throw new System.NotImplementedException();
            }
        }

        Matrix4x4 GetClipMatrix()
        {
            var proj = GetProjectionMatrix();
            var pos = (pixelPerfect) ? (Vector3.Round(gameObject.transform.position)) : (gameObject.transform.position);
            var prs = Matrix4x4.PR(pos, gameObject.transform.rotation);

            return prs * proj;
        }

        public void Render(List<Renderer> renderables)
        {
            switch (clearFlags)
            {
                case CameraClearFlags.SolidColor:
                    Application.currentScreen.Clear(backgroundColor);
                    break;
                case CameraClearFlags.Depth:
                    break;
                case CameraClearFlags.Nothing:
                    break;
                default:
                    break;
            }

            var clipMatrix = GetClipMatrix();

            foreach (var renderable in renderables)
            {
                renderable.Render(clipMatrix);
            }
        }
    }
}
