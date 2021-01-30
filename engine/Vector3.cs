
namespace SoftRender
{
    // Basic implementation of a 3D vector
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        public static Vector3 operator *(Vector3 v, float s) => new Vector3(v.x * s, v.y * s, v.z * s);
        public static Vector3 operator *(float s, Vector3 v) => new Vector3(v.x * s, v.y * s, v.z * s);
        public static Vector3 operator /(Vector3 v, float s) => new Vector3(v.x / s, v.y / s, v.z / s);

        public float magnitude
        {
            get { return Mathf.Sqrt(x * x + y * y + z * z); }
        }

        public Vector3 normalized
        {
            get { return this * (1.0f / magnitude); }
        }

        public override string ToString()
        {
            return $"({x:F3},{y:F3},{z:F3})";
        }
    }
}
