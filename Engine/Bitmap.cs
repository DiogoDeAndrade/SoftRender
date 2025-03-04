using System;
using System.IO;
using System.Threading.Tasks;
using Mathlib;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Color = Mathlib.Color;

namespace SoftRender.Engine
{
    public struct FatVertex
    {
        public Vector4      position;
        public Vector3      normal;
        public Vector3      tangent;
        public Vector3      binormal;
        public Color        color0;
        public Color        color1;
        public Vector2      uv0;
        public Vector2      uv1;

        public void Interpolate(FatVertex v0, FatVertex edge0, FatVertex edge1, float w1, float w2)
        {
            position = v0.position + edge0.position * w1 + edge1.position * w2;
            normal = v0.normal + edge0.normal * w1 + edge1.normal * w2;
            tangent = v0.tangent + edge0.tangent * w1 + edge1.tangent * w2;
            binormal = v0.binormal + edge0.binormal * w1 + edge1.binormal * w2;
            color0 = v0.color0 + edge0.color0 * w1 + edge1.color0 * w2;
            color1 = v0.color1 + edge0.color1 * w1 + edge1.color1 * w2;
            uv0 = v0.uv0 + edge0.uv0 * w1 + edge1.uv0 * w2;
            uv1 = v0.uv1 + edge0.uv1 * w1 + edge1.uv1 * w2;
        }

        public void Interpolate(FatVertex v0, FatVertex v1, FatVertex v2, float w0, float w1, float w2)
        {
            position = v0.position * w0 + v1.position * w1 + v2.position * w2;
            normal = v0.normal * w0 + v1.normal * w1 + v2.normal * w2;
            tangent = v0.tangent * w0 + v1.tangent * w1 + v2.tangent * w2;
            binormal = v0.binormal * w0 + v1.binormal * w1 + v2.binormal * w2;
            color0 = v0.color0 * w0 + v1.color0 * w1 + v2.color0 * w2;
            color1 = v0.color1 * w0 + v1.color1 * w1 + v2.color1 * w2;
            uv0 = v0.uv0 * w0 + v1.uv0 * w1 + v2.uv0 * w2;
            uv1 = v0.uv1 * w0 + v1.uv1 * w1 + v2.uv1 * w2;
        }

        public void InterpolatePC(FatVertex v0, FatVertex v1, FatVertex v2, float w0, float w1, float w2)
        {
            position = v0.position * w0 + v1.position * w1 + v2.position * w2;

            float invZ = 1 / position.z;

            normal = (v0.normal * w0 + v1.normal * w1 + v2.normal * w2) *invZ; ;
            tangent = (v0.tangent * w0 + v1.tangent * w1 + v2.tangent * w2) *invZ; ;
            binormal = (v0.binormal * w0 + v1.binormal * w1 + v2.binormal * w2) *invZ; ;
            color0 = (v0.color0 * w0 + v1.color0 * w1 + v2.color0 * w2) *invZ; ;
            color1 = (v0.color1 * w0 + v1.color1 * w1 + v2.color1 * w2) *invZ; ;
            uv0 = (v0.uv0 * w0 + v1.uv0 * w1 + v2.uv0 * w2) * invZ;
            uv1 = (v0.uv1 * w0 + v1.uv1 * w1 + v2.uv1 * w2) *invZ; ;
        }

        public void MultiplyAttributes(float v)
        {
            normal *= v;
            tangent *= v;
            binormal *= v;
            color0 *= v;
            color1 *= v;
            uv0 *= v;
            uv1 *= v;
        }
    };

    public class Bitmap
    {
        public Color32[]    data;
        public float[]      depthBuffer;
        public int          width, height;

        public Bitmap()
        {
            width = height = 0;
        }

        public Bitmap(int width, int height)
        {
            this.width = width;
            this.height = height;
            data = new Color32[width * height];
        }
        public Bitmap(string filename)
        {
            this.width = 0;
            this.height = 0;
            data = null;

            Load(filename);
        }

        public void AttachDepthBuffer()
        {
            depthBuffer = new float[width * height];
        }

        public void Clear(Color color, float depth = 1)
        {
            Clear((Color32)color, depth);
        }

        public void Clear(Color32 color, float depth = 1)
        {
            var span = new Span<Color32>(data);
            span.Fill(color);

            if (depthBuffer != null)
            {
                var spanZ = new Span<float>(depthBuffer);
                spanZ.Fill(depth);
            }
        }

        public void Clear(float depth = 1)
        {
            var spanZ = new Span<float>(depthBuffer);
            spanZ.Fill(depth);
        }

        public void DrawRect(Rect rect, Color32 color)
        {
            int x1 = Math.Max((int)rect.x1, 0); x1 = Math.Min(x1, width - 1);
            int y1 = Math.Max((int)rect.y1, 0); y1 = Math.Min(y1, height - 1);
            int x2 = Math.Max((int)rect.x2, 0); x2 = Math.Min(x2, width - 1);
            int y2 = Math.Max((int)rect.y2, 0); y2 = Math.Min(y2, height - 1);

            for (int y = y1; y < y2; y++)
            {
                int idx = x1 + y * width;
                for (int x = x1; x < x2; x++)
                {
                    data[idx] = color;
                    idx++;
                }
            }
        }

        public void DrawLine(Vector2 p1, Vector2 p2, Color color)
        {
            DrawLine(p1, p2, (Color32)color);
        }

        public void DrawLine(Vector2 p1, Vector2 p2, Color32 color)
        {
            bool is_inside1 = (p1.x > 0) && (p1.x < width) && (p1.y > 0) && (p1.y < height);
            bool is_inside2 = (p2.x > 0) && (p2.x < width) && (p2.y > 0) && (p2.y < height);
            // Check if the line needs clipping
            if (is_inside1 && is_inside2)
            {
                // It's completely inside the viewport
                DrawLine_Unclipped(p1, p2, color);
            }
            else
            {
                var pStart = p1;
                var pEnd = p2;

                // Both points outside
                if (!is_inside1 && !is_inside2)
                {
                    if ((p1.x < 0) && (p2.x < 0)) return;
                    if ((p1.y < 0) && (p2.y < 0)) return;
                    if ((p1.x >= width) && (p2.x >= width)) return;
                    if ((p1.y >= height) && (p2.y >= height)) return;
                }
                if (!is_inside1)
                {
                    Vector2 delta = p1 - p2;

                    // Find intersection of line with the 4 boundaries
                    float t = 1.0f;
                    float test = (0 - p2.x) / delta.x;
                    t = (test >= 0) ? (Mathf.Min(t, test)) : (t);
                    test = ((width - 1) - p2.x) / delta.x;
                    t = (test >= 0) ? (Mathf.Min(t, test)) : (t);
                    test = (0 - p2.y) / delta.y;
                    t = (test >= 0) ? (Mathf.Min(t, test)) : (t);
                    test = ((height - 1) - p2.y) / delta.y;
                    t = (test >= 0) ? (Mathf.Min(t, test)) : (t);

                    pStart = p2 + t * delta;
                }
                if (!is_inside2)
                {
                    Vector2 delta = p2 - p1;

                    // Find intersection of line with the 4 boundaries
                    float t = 1.0f;
                    float test = (0 - p1.x) / delta.x;
                    t = (test >= 0) ? (Mathf.Min(t, test)) : (t);
                    test = ((width - 1) - p1.x) / delta.x;
                    t = (test >= 0) ? (Mathf.Min(t, test)) : (t);
                    test = (0 - p1.y) / delta.y;
                    t = (test >= 0) ? (Mathf.Min(t, test)) : (t);
                    test = ((height - 1) - p1.y) / delta.y;
                    t = (test >= 0) ? (Mathf.Min(t, test)) : (t);

                    pEnd = p1 + t * delta;
                }

                // Recheck if points are inside
                if ((pStart.x < 0) || (pStart.x >= width) || (pStart.y < 0) || (pStart.y >= height) ||
                    (pEnd.x < 0) || (pEnd.x >= width) || (pEnd.y < 0) || (pEnd.y >= height))
                {
                    // Clipped line is completely outside
                    return;
                }
                else
                {
                    DrawLine_Unclipped(pStart, pEnd, color);
                }
            }
        }

        public void DrawLine_Unclipped(Vector2 p1, Vector2 p2, Color color)
        {
            DrawLine_Unclipped(p1, p2, (Color32)color);

        }
        public void DrawLine_Unclipped(Vector2 p1, Vector2 p2, Color32 color)
        {
            float dx = Mathf.Abs(p1.x - p2.x);
            float dy = Mathf.Abs(p1.y - p2.y);

            if (dx > dy)
            {
                // Iterate on horizontal
                int x1, y1, x2, y2;
                if (p1.x < p2.x)
                {
                    x1 = (int)p1.x;
                    y1 = (int)p1.y;
                    x2 = (int)p2.x;
                    y2 = (int)p2.y;
                }
                else
                {
                    x1 = (int)p2.x;
                    y1 = (int)p2.y;
                    x2 = (int)p1.x;
                    y2 = (int)p1.y;
                }

                float y = y1;
                float incY = (y2 - y1) / (float)(x2 - x1);
                for (int x = x1; x <= x2; x++)
                {
                    data[x + (int)y * width] = color;
                    y = y + incY;
                }
            }
            else
            {
                // Iterate on vertical
                int x1, y1, x2, y2;
                if (p1.y < p2.y)
                {
                    x1 = (int)p1.x;
                    y1 = (int)p1.y;
                    x2 = (int)p2.x;
                    y2 = (int)p2.y;
                }
                else
                {
                    x1 = (int)p2.x;
                    y1 = (int)p2.y;
                    x2 = (int)p1.x;
                    y2 = (int)p1.y;
                }

                float x = x1;
                float incX = (x2 - x1) / (float)(y2 - y1);
                for (int y = y1; y <= y2; y++)
                {
                    data[(int)x + y * width] = color;
                    x = x + incX;
                }
            }
        }

        public void DrawTriangle(Vector2 p1, Vector2 p2, Vector2 p3, Color color)
        {
            DrawTriangleScanline(p1, p2, p3, (Color32)color);
        }

        public void DrawTriangle(Vector2 p1, Vector2 p2, Vector2 p3, Color32 color)
        {
            DrawTriangleScanline(p1, p2, p3, color);
        }

        public void DrawTriangle(FatVertex p1, FatVertex p2, FatVertex p3, Material material)
        {
            // Do the near/far culling - for now just eliminate triangles completely
            if (p1.position.w <= 0) return;
            if (p2.position.w <= 0) return;
            if (p3.position.w <= 0) return;

            DrawTriangleEdgeFunction(p1, p2, p3, material);
        }

        public void DrawTriangleScanline(Vector2 p1, Vector2 p2, Vector2 p3, Color32 color)
        {
            Vector2[] p = new Vector2[] { p1, p2, p3 };

            // Find smallest Y
            int minIndexY = 0;
            if (p[minIndexY].y > p[1].y) minIndexY = 1;
            if (p[minIndexY].y > p[2].y) minIndexY = 2;

            // Find edge X
            int minIndexX, maxIndexX;
            minIndexX = maxIndexX = -1;

            for (int i = 0; i < 3; i++)
            {
                if (i == minIndexY) continue;
                if ((minIndexX == -1) || (p[minIndexX].x > p[i].x)) minIndexX = i;
                if ((maxIndexX == -1) || (p[maxIndexX].x < p[i].x)) maxIndexX = i;
            }

            // Find Y limits
            int midIndexY, maxIndexY;
            if (p[minIndexX].y < p[maxIndexX].y)
            {
                midIndexY = minIndexX;
                maxIndexY = maxIndexX;
            }
            else
            {
                midIndexY = maxIndexX;
                maxIndexY = minIndexX;
            }

            // We start at minY and go down on both edges minX and maxX
            // Then, at midY, we recompute one of the edges that's not midY and minY
            int y1 = (int)p[minIndexY].y;
            int y2 = (int)p[midIndexY].y;
            int y3 = (int)p[maxIndexY].y;

            float minX, maxX;
            minX = maxX = p[minIndexY].x;

            float incMinX = (p[minIndexX].x - minX) / (p[minIndexX].y - p[minIndexY].y);
            float incMaxX = (p[maxIndexX].x - maxX) / (p[maxIndexX].y - p[minIndexY].y);

            bool earlyOut = false;
            if (y2 > height) { y2 = height - 1; earlyOut = true; }

            // Special case: horizontal edge on top
            if ((int)p[minIndexY].y == (int)p[minIndexX].y)
            {
                earlyOut = true;
                midIndexY = maxIndexX;
                y2 = (int)Mathf.Min(p[midIndexY].y + 1, height - 1);
                if (p[minIndexY].x < p[minIndexX].x) { minX = p[minIndexY].x; maxX = p[minIndexX].x; }
                else { minX = p[minIndexX].x; maxX = p[minIndexY].x; }

                incMinX = (p[midIndexY].x - minX) / (p[midIndexY].y - p[minIndexY].y);
                incMaxX = (p[midIndexY].x - maxX) / (p[midIndexY].y - p[minIndexY].y);
            }
            else if ((int)p[minIndexY].y == (int)p[maxIndexX].y)
            {
                earlyOut = true;
                midIndexY = minIndexX;
                y2 = (int)Mathf.Min(p[midIndexY].y + 1, height - 1);
                if (p[minIndexY].x < p[maxIndexX].x) { minX = p[minIndexY].x; maxX = p[maxIndexX].x; }
                else { minX = p[maxIndexX].x; maxX = p[minIndexY].x; }

                incMinX = (p[midIndexY].x - minX) / (p[midIndexY].y - p[minIndexY].y);
                incMaxX = (p[midIndexY].x - maxX) / (p[midIndexY].y - p[minIndexY].y);
            }

            for (int y = y1; y < y2; y++)
            {
                if (y >= 0)
                {
                    // Fill span
                    int m1 = (minX >= 0) ? ((int)minX) : (0);
                    int m2 = (maxX < width) ? ((int)maxX) : (width - 1);

                    int idx = y * width + m1;
                    for (int x = m1; x <= m2; x++)
                    {
                        data[idx] = color;
                        idx++;
                    }
                }

                minX = minX + incMinX;
                maxX = maxX + incMaxX;
            }

            // Out of the bottom of the screen, no point in more calculations
            if (earlyOut) return;

            if (minIndexX == midIndexY)
            {
                incMinX = (p[maxIndexX].x - minX) / (y3 - y2);
            }
            else
            {
                incMaxX = (p[minIndexX].x - maxX) / (y3 - y2);
            }

            if (y3 >= height) y3 = height - 1;

            for (int y = y2; y <= y3; y++)
            {
                if (y >= 0)
                {
                    // Fill span
                    int m1 = (minX >= 0) ? ((int)minX) : (0);
                    int m2 = (maxX < width) ? ((int)maxX) : (width - 1);

                    int idx = y * width + m1;
                    for (int x = m1; x <= m2; x++)
                    {
                        data[idx] = color;
                        idx++;
                    }
                }

                minX = minX + incMinX;
                maxX = maxX + incMaxX;
            }
        }
        public void DrawTriangleScanline(FatVertex p1, FatVertex p2, FatVertex p3, Material material)
        {
            Vector2[]   p = new Vector2[] { Vector2.FloorToInt(p1.position.xy), Vector2.FloorToInt(p2.position.xy), Vector2.FloorToInt(p3.position.xy) };
            float[]     z = new float[] { p1.position.z, p2.position.z, p3.position.z };
            Color[]     c = new Color[] { p1.color0, p2.color0, p3.color0 };

            // Find smallest Y
            int minIndexY = 0;
            if (p[minIndexY].y > p[1].y) minIndexY = 1;
            if (p[minIndexY].y > p[2].y) minIndexY = 2;

            // Find edge X
            int minIndexX, maxIndexX;
            minIndexX = maxIndexX = -1;

            for (int i = 0; i < 3; i++)
            {
                if (i == minIndexY) continue;
                if ((minIndexX == -1) || (p[minIndexX].x > p[i].x)) minIndexX = i;
                if ((maxIndexX == -1) || (p[maxIndexX].x < p[i].x)) maxIndexX = i;
            }

            // Check if minIndexX == maxIndexX(there is a vertical edge, need to solve for it)
            if (minIndexX == maxIndexX)
            {
                for (int i = 0; i < 3; i++)
                {
                    if ((i == minIndexX) || (i == minIndexY)) continue;
                    maxIndexX = i;
                    break;
                }
            }

            // Find Y limits
            int midIndexY, maxIndexY;
            if (p[minIndexX].y < p[maxIndexX].y)
            {
                midIndexY = minIndexX;
                maxIndexY = maxIndexX;
            }
            else
            {
                midIndexY = maxIndexX;
                maxIndexY = minIndexX;
            }

            // We start at minY and go down on both edges minX and maxX
            // Then, at midY, we recompute one of the edges that's not midY and minY
            int y1 = (int)p[minIndexY].y;
            int y2 = (int)p[midIndexY].y;
            int y3 = (int)p[maxIndexY].y;

            float minX, maxX;
            Color minC, maxC;
            float minZ, maxZ;

            minX = maxX = p[minIndexY].x;
            minC = maxC = c[minIndexY];
            minZ = maxZ = z[minIndexY];

            float incMinX = (p[minIndexX].x - minX) / (p[minIndexX].y - p[minIndexY].y);
            float incMaxX = (p[maxIndexX].x - maxX) / (p[maxIndexX].y - p[minIndexY].y);
            Color incMinC = (c[minIndexX] - minC) / (p[minIndexX].y - p[minIndexY].y);
            Color incMaxC = (c[maxIndexX] - maxC) / (p[maxIndexX].y - p[minIndexY].y);
            float incMinZ = (z[minIndexX] - minZ) / (p[minIndexX].y - p[minIndexY].y);
            float incMaxZ = (z[maxIndexX] - maxZ) / (p[maxIndexX].y - p[minIndexY].y);

            bool earlyOut = false;
            if (y2 > height) { y2 = height - 1; earlyOut = true; }

            // Special case: horizontal edge on top
            if ((int)p[minIndexY].y == (int)p[minIndexX].y)
            {
                earlyOut = true;
                midIndexY = maxIndexX;
                y2 = (int)Mathf.Min(p[midIndexY].y + 1, height - 1);
                if (p[minIndexY].x < p[minIndexX].x) { minX = p[minIndexY].x; maxX = p[minIndexX].x; minC = c[minIndexY]; maxC = c[minIndexX]; minZ = z[minIndexY]; maxZ = z[minIndexX]; }
                else { minX = p[minIndexX].x; maxX = p[minIndexY].x; minC = c[minIndexX]; maxC = c[minIndexX]; minZ = z[minIndexX]; maxZ = z[minIndexX]; }

                incMinX = (p[midIndexY].x - minX) / (p[midIndexY].y - p[minIndexY].y);
                incMaxX = (p[midIndexY].x - maxX) / (p[midIndexY].y - p[minIndexY].y);
                incMinC = (c[midIndexY] - minC) / (p[midIndexY].y - p[minIndexY].y);
                incMaxC = (c[midIndexY] - maxC) / (p[midIndexY].y - p[minIndexY].y);
                incMinZ = (z[midIndexY] - minZ) / (p[midIndexY].y - p[minIndexY].y);
                incMaxZ = (z[midIndexY] - maxZ) / (p[midIndexY].y - p[minIndexY].y);
            }
            else if ((int)p[minIndexY].y == (int)p[maxIndexX].y)
            {
                earlyOut = true;
                midIndexY = minIndexX;
                y2 = (int)Mathf.Min(p[midIndexY].y + 1, height - 1);
                if (p[minIndexY].x < p[maxIndexX].x) { minX = p[minIndexY].x; maxX = p[maxIndexX].x; minC = c[minIndexY]; maxC = c[maxIndexX]; minZ = z[minIndexY]; maxZ = z[maxIndexX]; }
                else { minX = p[maxIndexX].x; maxX = p[minIndexY].x; minC = c[maxIndexX]; maxC = c[minIndexY]; minZ = z[maxIndexX]; maxZ = z[minIndexY]; }

                incMinX = (p[midIndexY].x - minX) / (p[midIndexY].y - p[minIndexY].y);
                incMaxX = (p[midIndexY].x - maxX) / (p[midIndexY].y - p[minIndexY].y);
                incMinC = (c[midIndexY] - minC) / (p[midIndexY].y - p[minIndexY].y);
                incMaxC = (c[midIndexY] - maxC) / (p[midIndexY].y - p[minIndexY].y);
                incMinZ = (z[midIndexY] - minZ) / (p[midIndexY].y - p[minIndexY].y);
                incMaxZ = (z[midIndexY] - maxZ) / (p[midIndexY].y - p[minIndexY].y);
            }

            var depthTestAndSet = material.GetTestAndSetDepthFunction();
            var blendFunction = material.GetBlendFunction();

            for (int y = y1; y < y2; y++)
            {
                if (y >= 0)
                {
                    // Fill span
                    int startX = (int)minX;
                    Color startC = minC;
                    float startZ = minZ;
                    int endX = (int)maxX;
                    Color endC = maxC;
                    float endZ = maxZ;

                    if (startX > endX)
                    {
                        (startX, endX) = (endX, startX);
                        (startC, endC) = (endC, startC);
                        (startZ, endZ) = (endZ, startZ);
                    }

                    int m1 = startX;
                    int m2 = endX;

                    m1 = Mathf.Clamp(m1, 0, width - 1);
                    m2 = Mathf.Clamp(m2, 0, width - 1);
                    m2 = (m2 < width) ? (m2) : (width - 1);

                    int deltaX = m2 - m1;
                    float targetZ, incZ;
                    Color targetC, incC;

                    if (deltaX == 0) { targetZ = startZ; targetC = startC; incZ = 0; incC = Color.black; }
                    else
                    {
                        deltaX = (endX - startX);
                        float t = (m1 - startX) / deltaX;
                        targetZ = Mathf.Lerp(startZ, endZ, t);
                        incZ = (endZ - startZ) / deltaX;
                        targetC = Color.Lerp(startC, endC, t);
                        incC = (endC - startC) / deltaX;
                    }

                    int idx = y * width + m1;
                    for (int x = m1; x <= m2; x++)
                    {
                        if (depthTestAndSet(ref depthBuffer, idx, targetZ))
                        {
                            data[idx] = blendFunction((Color32)targetC, data[idx]);
                        }
                        targetZ = targetZ + incZ;
                        targetC = targetC + incC;
                        idx++;
                    }
                }

                minX = minX + incMinX; minC = minC + incMinC; minZ = minZ + incMinZ;
                maxX = maxX + incMaxX; maxC = maxC + incMaxC; maxZ = maxZ + incMaxZ;
            }

            // Out of the bottom of the screen, no point in more calculations
            if (earlyOut) return;

            if (minIndexX == midIndexY)
            {
                incMinX = (p[maxIndexX].x - minX) / (y3 - y2);
                incMinC = (c[maxIndexX] - minC) / (y3 - y2);
                incMinZ = (z[maxIndexX] - minZ) / (y3 - y2);
            }
            else
            {
                incMaxX = (p[minIndexX].x - maxX) / (y3 - y2);
                incMaxC = (c[minIndexX] - maxC) / (y3 - y2);
                incMaxZ = (z[minIndexX] - maxZ) / (y3 - y2);
            }

            if (y3 >= height) y3 = height - 1;

            for (int y = y2; y <= y3; y++)
            {
                if (y >= 0)
                {
                    // Fill span
                    int     startX = (int)minX;
                    Color   startC = minC;
                    float   startZ = minZ;
                    int     endX = (int)maxX;
                    Color   endC = maxC;
                    float   endZ = maxZ;

                    if (startX > endX)
                    {
                        (startX, endX) = (endX, startX);
                        (startC, endC) = (endC, startC);
                        (startZ, endZ) = (endZ, startZ);
                    }

                    int m1 = startX;
                    int m2 = endX;

                    m1 = Mathf.Clamp(m1, 0, width - 1);
                    m2 = Mathf.Clamp(m2, 0, width - 1);
                    m2 = (m2 < width) ? (m2) : (width - 1);

                    int     deltaX = m2 - m1;
                    float   targetZ, incZ;
                    Color   targetC, incC;

                    if (deltaX == 0) { targetZ = startZ; targetC = startC; incZ = 0; incC = Color.black; }
                    else
                    {
                        deltaX = (endX - startX);
                        float t = (m1 - startX) / deltaX;
                        targetZ = Mathf.Lerp(startZ, endZ, t);
                        incZ = (endZ - startZ) / deltaX;
                        targetC = Color.Lerp(startC, endC, t);
                        incC = (endC - startC) / deltaX;
                    }

                    int idx = y * width + m1;
                    for (int x = m1; x <= m2; x++)
                    {
                        if (depthTestAndSet(ref depthBuffer, idx, targetZ))
                        {
                            data[idx] = blendFunction((Color32)targetC, data[idx]);
                        }
                        targetZ = targetZ + incZ;
                        targetC = targetC + incC;
                        idx++;
                    }
                }

                minX = minX + incMinX; minC = minC + incMinC; minZ = minZ + incMinZ;
                maxX = maxX + incMaxX; maxC = maxC + incMaxC; maxZ = maxZ + incMaxZ;
            }
        }

        delegate bool EdgeFunctionValidation(float e1, float e2, float e3);

        public void DrawTriangleEdgeFunction(FatVertex p0, FatVertex p1, FatVertex p2, Material material)
        {
            // Create edge function validation function
            EdgeFunctionValidation edgeFunctionValidation;
            switch (material.cullMode)
            {
                case CullMode.Front:
                    edgeFunctionValidation = (e1, e2, e3) => (e1 >= 0) && (e2 >= 0) && (e3 >= 0);
                    break;
                case CullMode.Back:
                    edgeFunctionValidation = (e1, e2, e3) => (e1 <= 0) && (e2 <= 0) && (e3 <= 0);
                    break;
                case CullMode.Off:
                default:
                    edgeFunctionValidation = (e1, e2, e3) => ((e1 <= 0) && (e2 <= 0) && (e3 <= 0)) || ((e1 >= 0) && (e2 >= 0) && (e3 >= 0));
                    break;
            }

            // Get bounding box of triangle
            Rect r = new Rect(p0.position.xy);
            r.ExtendTo(p1.position.xy);
            r.ExtendTo(p2.position.xy);

            int x1 = Mathf.FloorToInt(r.x1);
            int y1 = Mathf.FloorToInt(r.y1);
            int x2 = Mathf.CeilToInt(r.x2);
            int y2 = Mathf.CeilToInt(r.y2);

            // Clip the rectangle to the screen
            x1 = Mathf.Clamp(x1, 0, width - 1);
            y1 = Mathf.Clamp(y1, 0, height - 1);
            x2 = Mathf.Clamp(x2, 0, width - 1);
            y2 = Mathf.Clamp(y2, 0, height - 1);

            if (x1 == x2) return;
            if (y1 == y2) return;

            // Setup auxiliary variables
            Vector2 edge0 = p2.position.xy - p1.position.xy;
            Vector2 edge1 = p0.position.xy - p2.position.xy;
            Vector2 edge2 = p1.position.xy - p0.position.xy;
            float   area = Triangle.GetArea2x(p0.position.xy, p1.position.xy, p2.position.xy);

            var depthTestAndSet = material.GetTestAndSetDepthFunction();
            var blendFunction = material.GetBlendFunction();

            var fragmentProgram = material.shader.GetFragmentProgram();

            // On all pixels of the bounding rectangle
            Parallel.For(y1, y2 + 1, (y) =>
            {
                FatVertex current = new FatVertex();

                // Compute partials for the edge function
                float e0y = (y - p2.position.y) * edge0.x;
                float e1y = (y - p0.position.y) * edge1.x;
                float e2y = (y - p1.position.y) * edge2.x;

                int baseIdx = y * width;
                for (int x = x1; x <= x2; x++)
                {
                    int idx = baseIdx + x;

                    // Compute the barycentric coordinates
                    float e0 = (x - p2.position.x) * edge0.y - e0y;
                    float e1 = (x - p0.position.x) * edge1.y - e1y;
                    float e2 = (x - p1.position.x) * edge2.y - e2y;

                    if (edgeFunctionValidation(e0, e1, e2))
                    {
                        // Finish the barycentric coordinates (normalization)
                        e0 /= area; e1 /= area; e2 /= area;

                        current.InterpolatePC(p0, p1, p2, e0, e1, e2);

                        if (depthTestAndSet(ref depthBuffer, idx, 1.0f / current.position.z))
                        {
                            Color c = fragmentProgram(current);
                            data[idx] = blendFunction((Color32)c, data[idx]);
                        }
                    }
                }
            });
        }

        public bool Load(string filename)
        {
            try
            {
                using (Image<Bgra32> image = Image.Load<Bgra32>(filename))
                {
                    width = image.Width;
                    height = image.Height;

                    data = new Color32[width * height];

                    for (int y = 0; y < height; y++)
                    {
                        int targetIndex = y * width;
                        for (int x = 0; x < width; x++)
                        {
                            Bgra32 pixel = image[x, y]; // Access pixels directly
                            data[targetIndex] = new Color32(pixel.R, pixel.G, pixel.B, pixel.A);
                            targetIndex++;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading image: " + ex.Message);
                return false;
            }
        }

        public void BlitWithAlphablend(int x, int y, Bitmap src)
        {
            int srcWidth = src.width;
            int srcHeight = src.height;

            // Check if completely out of the screen
            if (((x + srcWidth) < 0) ||
                ((y + srcHeight) < 0) ||
                (x >= width) ||
                (y >= height)) return;

            int startX = x;
            int startY = y;
            int endY = y + srcHeight;

            for (int yy = startY; yy < endY; yy++)
            {
                if (yy < 0) continue;
                else if (yy >= height) break;

                int destIndex = x + yy * width;
                int srcIndex = (yy - startY) * srcWidth;
                int endX = startX + srcWidth;

                for (int xx = startX; xx < endX; xx++)
                {
                    if (xx >= 0)
                    {
                        if (xx >= width) break;

                        data[destIndex] = Color32.Lerp(data[destIndex], src.data[srcIndex], src.data[srcIndex].a);
                    }
                    destIndex++;
                    srcIndex++;
                }
            }
        }

        public void BlitWithAlphablend(int x, int y, Bitmap src, Rect srcRect)
        {
            int srcWidth = (int)srcRect.width;
            int srcHeight = (int)srcRect.height;

            // Check if completely out of the screen
            if (((x + srcWidth) < 0) ||
                ((y + srcHeight) < 0) ||
                (x >= width) ||
                (y >= height)) return;

            int startX = x;
            int startY = y;
            int endY = y + srcHeight;

            for (int yy = startY; yy < endY; yy++)
            {
                if (yy < 0) continue;
                else if (yy >= height) break;

                int destIndex = x + yy * width;
                int srcIndex = (yy - startY + (int)srcRect.y1) * src.width + (int)srcRect.x1;
                int endX = startX + srcWidth;

                for (int xx = startX; xx < endX; xx++)
                {
                    if (xx >= 0)
                    {
                        if (xx >= width) break;

                        data[destIndex] = Color32.Lerp(data[destIndex], src.data[srcIndex], src.data[srcIndex].a);
                    }
                    destIndex++;
                    srcIndex++;
                }
            }
        }

        public void BlitMask(int x, int y, Bitmap src, Rect srcRect, Color32 c)
        {
            int srcWidth = (int)srcRect.width;
            int srcHeight = (int)srcRect.height;

            // Check if completely out of the screen
            if (((x + srcWidth) < 0) ||
                ((y + srcHeight) < 0) ||
                (x >= width) ||
                (y >= height)) return;

            int startX = x;
            int startY = y;
            int endY = y + srcHeight;

            for (int yy = startY; yy < endY; yy++)
            {
                if (yy < 0) continue;
                else if (yy >= height) break;

                int destIndex = x + yy * width;
                int srcIndex = (yy - startY + (int)srcRect.y1) * src.width + (int)srcRect.x1;
                int endX = startX + srcWidth;

                for (int xx = startX; xx < endX; xx++)
                {
                    if (xx >= 0)
                    {
                        if (xx >= width) break;

                        data[destIndex] = Color32.Lerp(data[destIndex], c, src.data[srcIndex].a);
                    }
                    destIndex++;
                    srcIndex++;
                }
            }
        }

        public void BlitScale(Bitmap src, int x, int y, float scale)
        {
            int p = (int)(1 / scale);

            float srcX = 0.0f;
            float srcY = 0.0f;
            float inc = scale;

            for (int py = 0; py < src.height * p; py++)
            {
                int destIdx = x + py * width;
                int srcLineIdx = Mathf.FloorToInt(srcY) * src.width;
                
                srcX = 0.0f;

                for (int px = 0; px < src.width * p; px++)
                {
                    int srcIdx = Mathf.FloorToInt(srcX) + srcLineIdx;
                    data[destIdx] = src.data[srcIdx];

                    destIdx++;
                    srcX = srcX + inc;
                }

                srcY = srcY + inc;
            }
        }

        public void SwapRB()
        {
            for (int i = 0; i < width * height; i++)
            {
                data[i].Set(data[i].b, data[i].g, data[i].r, data[i].a);
            }
        }

        public void SwapRB(Bitmap src)
        {
            for (int i = 0; i < width * height; i++)
            {
                data[i].Set(src.data[i].b, src.data[i].g, src.data[i].r, src.data[i].a);
            }
        }

        public Rect GetFrameRect(int tx, int ty, int frameNumber)
        {
            var sx = width / tx;
            var sy = height / ty;
            var r = new Rect(sx * (frameNumber % tx), sy * (frameNumber / tx), 0, 0);
            r.x2 = r.x1 + sx;
            r.y2 = r.y1 + sy;

            return r;
        }

        public void Write(int x, int y, string txt, Font font, Color textColor)
        {
            Write(x, y, txt, font, (Color32)textColor);
        }
        public void Write(int x, int y, string txt, Font font, Color textColor, Color backgroundColor)
        {
            Write(x, y, txt, font, (Color32)textColor, (Color32)backgroundColor);
        }

        public void Write(int x, int y, string txt, Font font, Color32 textColor)
        {
            font.WriteTo(this, x, y, txt, textColor);
        }

        public void Write(int x, int y, string txt, Font font, Color32 textColor, Color32 backgroundColor)
        {
            Rect rect = font.GetTextRect(x, y, txt);

            DrawRect(rect, backgroundColor);
            
            font.WriteTo(this, x, y, txt, textColor);
        }
    }
}
