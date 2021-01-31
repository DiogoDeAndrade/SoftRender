
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
        public static Vector4 operator +(Vector4 a, Vector4 b) => new Vector4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        public static Vector4 operator -(Vector4 a, Vector4 b) => new Vector4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        public static Vector4 operator *(Vector4 v, float s) => new Vector4(v.x * s, v.y * s, v.z * s, v.w * s);
        public static Vector4 operator *(float s, Vector4 v) => new Vector4(v.x * s, v.y * s, v.z * s, v.w * s);
        public static Vector4 operator /(Vector4 v, float s) => new Vector4(v.x / s, v.y / s, v.z / s, v.w / s);

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
    }
}
