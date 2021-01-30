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
    }
}
