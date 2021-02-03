using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRender.Engine
{
    public static class Mathf
    {
        public static float PI = MathF.PI;
        public static float Deg2Rad = (MathF.PI * 2.0f) / 360.0f;
        public static float Rad2Deg = 360.0f / (MathF.PI * 2.0f);

        public static float Sin(float a) { return MathF.Sin(a); }
        public static float Cos(float a) { return MathF.Cos(a); }
        public static float Asin(float a) { return MathF.Asin(a); }
        public static float Clamp01(float v) { return (v < 0) ? (0) : (v > 1) ? (1) : (v); }
        public static float Abs(float v) { return MathF.Abs(v); }
        public static float Min(float a, float b) { return MathF.Min(a, b); }
        public static float Max(float a, float b) { return MathF.Max(a, b); }
        public static float Sqrt(float v) { return MathF.Sqrt(v); }
        public static float Floor(float v) { return MathF.Floor(v); }
        public static int FloorToInt(float v) { return (int)MathF.Floor(v); }
        public static float Round(float v) { return MathF.Round(v); }
        public static float Atan2(float y, float x) { return MathF.Atan2(y, x); }
        public static float CopySign(float x, float y) { return MathF.CopySign(x, y); }
    }
}
