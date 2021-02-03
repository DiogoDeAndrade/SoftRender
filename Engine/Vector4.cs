
namespace SoftRender.Engine
{
    // Basic implementation of a 4D vector
    public struct Vector4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public Vector4(Vector3 v, float w)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
            this.w = w;
        }
        public static Vector4 operator +(Vector4 a, Vector4 b) => new Vector4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        public static Vector4 operator -(Vector4 a, Vector4 b) => new Vector4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        public static Vector4 operator -(Vector4 a) => new Vector4(-a.x, -a.y, -a.z, -a.w);
        public static Vector4 operator *(Vector4 v, float s) => new Vector4(v.x * s, v.y * s, v.z * s, v.w * s);
        public static Vector4 operator *(float s, Vector4 v) => new Vector4(v.x * s, v.y * s, v.z * s, v.w * s);
        public static Vector4 operator /(Vector4 v, float s) => new Vector4(v.x / s, v.y / s, v.z / s, v.w / s);

        static public float Dot(Vector4 a, Vector4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

        public float magnitude
        {
            get { return Mathf.Sqrt(x * x + y * y + z * z + w * w); }
        }

        public Vector4 normalized
        {
            get { return this * (1.0f / magnitude); }
        }

        public override string ToString()
        {
            return $"({x:F3},{y:F3},{z:F3},{w:F3})";
        }

        public Vector2 xy { get => new Vector2(x, y); set => (x, y) = (value.x, value.y); }
        public Vector2 xz { get => new Vector2(x, z); set => (x, z) = (value.x, value.y); }
        public Vector2 xw { get => new Vector2(x, w); set => (x, w) = (value.x, value.y); }
        public Vector2 yx { get => new Vector2(y, x); set => (y, x) = (value.x, value.y); }
        public Vector2 yz { get => new Vector2(y, z); set => (y, z) = (value.x, value.y); }
        public Vector2 yw { get => new Vector2(y, w); set => (y, w) = (value.x, value.y); }
        public Vector2 zx { get => new Vector2(z, x); set => (z, x) = (value.x, value.y); }
        public Vector2 zy { get => new Vector2(z, y); set => (z, y) = (value.x, value.y); }
        public Vector2 zw { get => new Vector2(z, w); set => (z, w) = (value.x, value.y); }

        public Vector3 xyz { get => new Vector3(x, y, z); set => (x, y, z) = (value.x, value.y, value.z); }

        public static Vector4 zero = new Vector4(0, 0, 0, 0);
        public static Vector4 one = new Vector4(1, 1, 1, 1);
    }
}
