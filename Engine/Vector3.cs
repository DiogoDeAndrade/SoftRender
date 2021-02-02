
namespace SoftRender.Engine
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
        public static Vector3 operator +(Vector3 a, Vector2 b) => new Vector3(a.x + b.x, a.y + b.y, a.z);
        public static Vector3 operator +(Vector2 a, Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, b.z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        public static Vector3 operator -(Vector3 a) => new Vector3(-a.x, -a.y, -a.z);
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

        static public Vector3 Round(Vector3 v) => new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));

        public Vector2 xy { get => new Vector2(x, y); set => (x, y) = (value.x, value.y); }
        public Vector2 xz { get => new Vector2(x, z); set => (x, z) = (value.x, value.y); }
        public Vector2 yx { get => new Vector2(y, x); set => (y, x) = (value.x, value.y); }
        public Vector2 yz { get => new Vector2(y, z); set => (y, z) = (value.x, value.y); }
        public Vector2 zx { get => new Vector2(z, x); set => (z, x) = (value.x, value.y); }
        public Vector2 zy { get => new Vector2(z, y); set => (z, y) = (value.x, value.y); }

        public static Vector3 zero = new Vector3(0, 0, 0);
        public static Vector3 one = new Vector3(1, 1, 1);
        public static Vector3 up = new Vector3(0, 1, 0);
        public static Vector3 right = new Vector3(1, 0, 0);
        public static Vector3 down = new Vector3(0, -1, 0);
        public static Vector3 left = new Vector3(-1, 0, 0);
        public static Vector3 forward = new Vector3(0, 0, 1);
        public static Vector3 back = new Vector3(0, 0, -1);
    }
}
