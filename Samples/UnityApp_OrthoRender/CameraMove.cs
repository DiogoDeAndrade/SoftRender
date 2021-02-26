using SoftRender.Engine;
using SoftRender.UnityApp;
using Mathlib;

namespace SoftRender.Samples.UnityApp.OrthoRender
{
    class CameraMove : MonoBehaviour
    {
        public float moveSpeed = 50.0f;
        public float rotateSpeed = 45.0f;

        void Update()
        {
            if (Input.GetKey(KeyCode.W)) transform.position += Vector2.up * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S)) transform.position += Vector2.down * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.A)) transform.position += Vector2.left * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D)) transform.position += Vector2.right * moveSpeed * Time.deltaTime;

            if (Input.GetKey(KeyCode.Q)) transform.rotation *= Quaternion.Euler(0, 0, -rotateSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.E)) transform.rotation *= Quaternion.Euler(0, 0, rotateSpeed * Time.deltaTime);
        }

        void OnPostRender()
        {
            SoftRender.UnityApp.Application.Write(0, 10, "Camera Position = " + transform.position, Color.white, Color.black);
            SoftRender.UnityApp.Application.Write(0, 20, "Camera Rotation = " + transform.rotation.eulerAngles, Color.white, Color.black);
        }
    }
}
