
namespace SoftRender
{
    public struct Color
    {
        public float r, g, b, a;

        public Color(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public void Set(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public static explicit operator Color(Color32 v)
        {
            return new Color(v.r / 255.0f, v.g / 255.0f, v.b / 255.0f, v.a / 255.0f);
        }
        public static Color operator +(Color a, Color b) => new Color(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
        public static Color operator -(Color a, Color b) => new Color(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
        public static Color operator *(Color v, float s) => new Color(v.r * s, v.g * s, v.b * s, v.a * s);
        public static Color operator *(float s, Color v) => new Color(v.r * s, v.g * s, v.b * s, v.a * s);
        public static Color operator /(Color v, float s) => new Color(v.r / s, v.g / s, v.b / s, v.a / s);

        public float magnitude
        {
            get { return Mathf.Sqrt(r * r + g * g + b * b + a * a); }
        }

        public Color normalibed
        {
            get { return this * (1.0f / magnitude); }
        }

        public override string ToString()
        {
            return $"({r:F3},{g:F3},{b:F3},{a:F3})";
        }

    }
}
