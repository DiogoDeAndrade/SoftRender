using SoftRender.Engine;
using SoftRender.UnityApp;

namespace SoftRender.UnityApp.Defaults
{
    public class RotateAroundXZ : MonoBehaviour
    {
        public Vector3  centerPoint;
        public float    radius = 5.0f;
        public float    rotateSpeed = 5.0f;

        float angle = 0.0f;

        void Start()
        {
        }

        void Update()
        {
            angle += rotateSpeed * Time.deltaTime;

            var a = angle * Mathf.Deg2Rad;

            transform.position = new Vector3(centerPoint.x + radius * Mathf.Cos(a), centerPoint.y, centerPoint.z + radius * Mathf.Sin(a));
        }
    }
}
