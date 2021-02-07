﻿using SoftRender.Engine;
using SoftRender.UnityApp;
using SoftRender.UnityApp.Defaults;

namespace SoftRender.Samples.UnityApp.ObjLoad
{
    class ObjLoad : SoftRender.UnityApp.Application
    {
        public ObjLoad()
        {
            name = "Object Load Sample - Unity Framework";
            writeFPS = true;
            enableDepthBuffer = true;
        }

        protected override bool Initialize()
        {
            if (!base.Initialize()) return false;

            string model = "ship";

            var mesh = Resources.Load<Mesh>(model + ".obj");
            MeshTools.CopyNormalsToColor0(mesh);

            var meshObject = new GameObject("Object");
            var meshFilter = meshObject.AddComponent<MeshFilter>();
            var meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material
            {
                isWireframe = false,
                baseColor = Color.yellow
            };
            meshFilter.mesh = mesh;

            // Wireframe object
            /*meshObject = new GameObject("Object");
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material
            {
                isWireframe = true,
                baseColor = Color.black
            };
            meshFilter.mesh = mesh;//*/

            mainCamera.gameObject.AddComponent<FPCameraMove>();
            mainCamera.transform.position = new Vector3(0.0f, 1.0f, -10.0f);
            mainCamera.pixelPerfect = true;
            mainCamera.orthographic = false;
            mainCamera.farClipPlane = 1000.0f;

            if (model == "castle")
            {
                mainCamera.transform.position = new Vector3(0.0f, 10.0f, -30.0f);
            }

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
