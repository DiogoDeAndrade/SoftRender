namespace SoftRender.Engine
{
    public static class Triangle
    {
        static public float GetArea(Vector2 a, Vector2 b, Vector2 c) => 0.5f * GetArea2x(a, b, c);
        static public float GetArea2x(Vector2 a, Vector2 b, Vector2 c) => (c.x - a.x) * (b.y - a.y) - (c.y - a.y) * (b.x - a.x);
    }
}
