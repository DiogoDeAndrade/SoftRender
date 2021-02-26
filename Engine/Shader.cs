using Mathlib;

namespace SoftRender.Engine
{
    public abstract class Shader
    {
        public delegate void    VertexProgram(FatVertex src, ref FatVertex dest);
        public delegate Color   FragmentProgram(FatVertex vertex);

        public abstract void Setup(Material material);

        public abstract VertexProgram GetVertexProgram();
        public abstract FragmentProgram GetFragmentProgram();

        static public Color PointSample(Bitmap bitmap, Vector2 uv)
        {
            int x = Mathf.FloorToInt(Mathf.Fract(uv.x) * (bitmap.width - 1));
            int y = Mathf.FloorToInt(Mathf.Fract(uv.y) * (bitmap.height - 1));

            return (Color)bitmap.data[x + y * bitmap.width];
        }
    }
}
