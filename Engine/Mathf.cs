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

        public static float Sin(float a) => MathF.Sin(a);
        public static float Cos(float a) => MathF.Cos(a);
        public static float Tan(float a) => MathF.Tan(a); 
        public static float Asin(float a) => MathF.Asin(a);
        public static int Clamp(int v, int a, int b) => (v < a) ? (a) : ((v > b) ? (b) : (v));
        public static float Clamp(float v, float a, float b) => (v < a) ? (a) : ((v > b) ? (b) : (v));
        public static float Clamp01(float v) => (v < 0) ? (0) : ((v > 1) ? (1) : (v));
        public static float Abs(float v) => MathF.Abs(v);
        public static byte Min(byte a, byte b) => (a < b) ? (a) : (b);
        public static byte Max(byte a, byte b) => (a > b) ? (a) : (b);
        public static float Min(float a, float b) => MathF.Min(a, b);
        public static float Max(float a, float b) => MathF.Max(a, b);
        public static float Sqrt(float v) => MathF.Sqrt(v);
        public static float Floor(float v) => MathF.Floor(v);
        public static int FloorToInt(float v) => (int)MathF.Floor(v);
        public static int CeilToInt(float v) => (int)MathF.Ceiling(v);
        public static float Round(float v) => MathF.Round(v);
        public static float Atan2(float y, float x) => MathF.Atan2(y, x);
        public static float CopySign(float x, float y) => MathF.CopySign(x, y);
        public static float Pow(float x, float y) => MathF.Pow(x, y);

        public static float Lerp(float v1, float v2, float t) => v1 + (v2 - v1) * t;

        // Perlin noise implementation (taken from Unity: https://github.com/Unity-Technologies/UnityCsReference/blob/master/Modules/TreeEditor/Includes/Perlin.cs)
        public static float Perlin(Vector2 p) => Perlin(p.x, p.y);
        public static float Perlin(float x, float y)
        {
            int bx0, bx1, by0, by1, b00, b10, b01, b11;
            float rx0, rx1, ry0, ry1, sx, sy, a, b, u, v;
            int i, j;

            setup(x, out bx0, out bx1, out rx0, out rx1);
            setup(y, out by0, out by1, out ry0, out ry1);

            i = p[bx0];
            j = p[bx1];

            b00 = p[i + by0];
            b10 = p[j + by0];
            b01 = p[i + by1];
            b11 = p[j + by1];

            sx = s_curve(rx0);
            sy = s_curve(ry0);

            u = at2(rx0, ry0, g2[b00, 0], g2[b00, 1]);
            v = at2(rx1, ry0, g2[b10, 0], g2[b10, 1]);
            a = lerp(sx, u, v);

            u = at2(rx0, ry1, g2[b01, 0], g2[b01, 1]);
            v = at2(rx1, ry1, g2[b11, 0], g2[b11, 1]);
            b = lerp(sx, u, v);

            return lerp(sy, a, b);
        }

        // Helpers for Perlin noise
        const int B = 0x100;
        const int BM = 0xff;
        const int N = 0x1000;

        static int[] p = new int[B + B + 2];
        static float[,] g3 = new float[B + B + 2, 3];
        static float[,] g2 = new float[B + B + 2, 2];
        static float[] g1 = new float[B + B + 2];

        static float s_curve(float t)
        {
            return t * t * (3.0F - 2.0F * t);
        }

        static float lerp(float t, float a, float b)
        {
            return a + t * (b - a);
        }

        static void setup(float value, out int b0, out int b1, out float r0, out float r1)
        {
            float t = value + N;
            b0 = ((int)t) & BM;
            b1 = (b0 + 1) & BM;
            r0 = t - (int)t;
            r1 = r0 - 1.0F;
        }

        static float at2(float rx, float ry, float x, float y) { return rx * x + ry * y; }      
    }
}
