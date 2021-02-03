using SoftRender.Engine;
using SoftRender.UnityApp;

namespace SoftRender.Samples.UnityApp.PerspectiveRender
{
    class CameraMove : MonoBehaviour
    {
        public float moveSpeed = 1.0f;
        public float rotateSpeed = 45.0f;

        void Update()
        {
            if (Input.GetKey(KeyCode.W)) transform.position += Vector2.up * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S)) transform.position += Vector2.down * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.A)) transform.position += Vector2.left * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D)) transform.position += Vector2.right * moveSpeed * Time.deltaTime;

            Vector3 eulerAngles = transform.rotation.eulerAngles;

            if (Input.GetKey(KeyCode.Q)) eulerAngles.y += -rotateSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.E)) eulerAngles.y += rotateSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.F)) eulerAngles.x += rotateSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.R)) eulerAngles.x += -rotateSpeed * Time.deltaTime;

            transform.rotation = Quaternion.Euler(eulerAngles);
        }

        void OnPostRender()
        {
            SoftRender.UnityApp.Application.Write(0, 10, "Camera Position = " + transform.position, Color.white, Color.black);
            SoftRender.UnityApp.Application.Write(0, 20, "Camera Rotation = " + transform.rotation.eulerAngles, Color.white, Color.black);
        }
    }
}
