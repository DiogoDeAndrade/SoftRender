using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRender
{
    static class Mathf
    {
        public static float Sin(float a) { return MathF.Sin(a); }
        public static float Cos(float a) { return MathF.Cos(a); }
        public static float Clamp01(float v) { return (v < 0) ? (0) : (v > 1) ? (1) : (v); }
        public static float Abs(float v) { return MathF.Abs(v); }
        public static float Min(float a, float b) { return MathF.Min(a, b); }
        public static float Max(float a, float b) { return MathF.Max(a, b); }
    }
}
