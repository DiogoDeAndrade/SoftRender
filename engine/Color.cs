
namespace SoftRender
{
    struct Color
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
    }
}
