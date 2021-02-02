using SoftRender.Engine;
using SoftRender.UnityApp;

namespace SoftRender.Samples.UnityApp.OrthoRender
{
    class CameraMove : MonoBehaviour
    {
        public float moveSpeed = 50.0f;

        void Update()
        {
            if (Input.GetKey(KeyCode.W)) transform.position += Vector2.up * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S)) transform.position += Vector2.down * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.A)) transform.position += Vector2.left * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D)) transform.position += Vector2.right * moveSpeed * Time.deltaTime;
        }
    }
}
