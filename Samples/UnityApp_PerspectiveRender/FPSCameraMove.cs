﻿using SoftRender.Engine;
using SoftRender.UnityApp;

namespace SoftRender.Samples.UnityApp.PerspectiveRender
{
    class FPSCameraMove : MonoBehaviour
    {
        public float moveSpeed = 1.0f;
        public float rotateSpeed = 45.0f;

        void Update()
        {
            if (Input.GetKey(KeyCode.W)) transform.position += transform.forward.x0z.normalized * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S)) transform.position += transform.back.x0z.normalized * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.A)) transform.position += transform.left.x0z.normalized * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D)) transform.position += transform.right.x0z.normalized * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.R)) transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.F)) transform.position += Vector3.down * moveSpeed * Time.deltaTime;

            Vector2 mouseDelta = Input.mouseDelta / 50.0f;

            Quaternion currentRotation = transform.rotation;

            transform.rotation = currentRotation;
        }

        void OnPostRender()
        {
            SoftRender.UnityApp.Application.Write(0, 10, "Camera Position = " + transform.position, Color.white, Color.black);
            SoftRender.UnityApp.Application.Write(0, 20, "Camera Rotation = " + transform.rotation.eulerAngles, Color.white, Color.black);
        }
    }
}
