
namespace SoftRender.Engine
{
    // Basic implementation of a 2D vector of integers
    public struct Vector2i
    {
        public int x;
        public int y;

        public Vector2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2i operator +(Vector2i a, Vector2i b) => new Vector2i(a.x + b.x, a.y + b.y);
        public static Vector2i operator +(Vector2i a, int b) => new Vector2i(a.x + b, a.y + b);
        public static Vector2i operator -(Vector2i a, Vector2i b) => new Vector2i(a.x - b.x, a.y - b.y);
        public static Vector2i operator -(Vector2i a) => new Vector2i(-a.x, -a.y);
        public static Vector2i operator *(Vector2i v, float s) => new Vector2i((int)(v.x * s), (int)(v.y * s));
        public static Vector2i operator *(float s, Vector2i v) => new Vector2i((int)(v.x * s), (int)(v.y * s));
        public static Vector2i operator /(Vector2i v, float s) => new Vector2i((int)(v.x / s), (int)(v.y / s));
        public static bool operator ==(Vector2i a, Vector2i b) => a.Equals(b);
        public static bool operator !=(Vector2i a, Vector2i b) => !a.Equals(b);

        public override bool Equals(object obj)
        {
            Vector2i tmp = (Vector2i)obj;
            return (tmp.x == x) && (tmp.y == y);
        }
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }

        static public float Dot(Vector2i a, Vector2i b) => a.x * b.x + a.y * b.y;

        public float magnitude
        {
            get { return Mathf.Sqrt(x * x + y * y); }
        }

        public Vector2i normalized
        {
            get { return this * (1.0f / magnitude); }
        }

        public override string ToString()
        {
            return $"({x:F3},{y:F3})";
        }

        public static Vector2i zero = new Vector2i(0,0);
        public static Vector2i one = new Vector2i(1,1);
        public static Vector2i up = new Vector2i(0, 1);
        public static Vector2i right = new Vector2i(1, 0);
        public static Vector2i down = new Vector2i(0, -1);
        public static Vector2i left = new Vector2i(-1, 0);
    }
}
