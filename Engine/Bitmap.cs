﻿using SixLabors.ImageSharp;
using System;
using System.IO;

namespace SoftRender.Engine
{
    public class Bitmap
    {
        public Color32[] data;
        public int width, height;

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

        public void Clear(Color color)
        {
            Clear((Color32)color);
        }

        public void Clear(Color32 color)
        {
            var span = new Span<Color32>(data);
            span.Fill(color);
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

        public void DrawTriangle(Vector2 p1, Vector2 p2, Vector2 p3, Color color)
        {
            DrawTriangle(p1, p2, p3, (Color32)color);
        }

        public void DrawTriangle(Vector2 p1, Vector2 p2, Vector2 p3, Color32 color)
        {
            /*DrawLine(p1, p2, new Color(1, 0, 0, 1));
            DrawLine(p2, p3, new Color(1, 0, 0, 1));
            DrawLine(p3, p1, new Color(1, 0, 0, 1));//*/

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
                        data[idx++] = color;
                    }
                }

                minX += incMinX;
                maxX += incMaxX;
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
                        data[idx++] = color;
                    }
                }

                minX += incMinX;
                maxX += incMaxX;
            }
        }

        bool Load(string filename)
        {
            try
            {
                using (Stream stream = File.OpenRead(filename))
                {
                    var image = Image.Load<SixLabors.ImageSharp.PixelFormats.Bgra32>(stream);

                    width = image.Width;
                    height = image.Height;

                    data = new Color32[width * height];
                    for (int y = 0; y < height; y++)
                    {
                        var rowData = image.Frames.RootFrame.GetPixelRowSpan(y);
                        var targetIndex = y * width;
                        for (int x = 0; x < width; x++)
                        {
                            data[targetIndex].Set(rowData[x].R, rowData[x].G, rowData[x].B, rowData[x].A);
                            targetIndex++;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log("Failed to load " + filename + ": " + exception.Message);
                return false;
            }

            return true;
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
                    srcX += inc;
                }

                srcY += inc;
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
