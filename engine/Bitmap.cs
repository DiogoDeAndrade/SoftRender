using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRender
{
    class Bitmap
    {
        public Color32[]    data;
        public int          width, height;

        public Bitmap(int width, int height)
        {
            this.width = width;
            this.height = height;
            data = new Color32[width * height];
        }

        public void Clear(Color color)
        {
            Clear((Color32)color);
        }

        public void Clear(Color32 color)
        {
            var span = new Span<Color32>(data);
            span.Fill(color);
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
                    y += incY;
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
                    x += incX;
                }
            }
        }
    }
}
