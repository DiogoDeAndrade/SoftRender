using SixLabors.ImageSharp;
using System;
using System.IO;

namespace SoftRender.Engine
{
    public class Font : Bitmap
    {
        int columns;
        int rows;
        int charWidth, charHeight;

        public Font(string filename, int columns = 18, int rows = 6) : base(filename)
        {
            this.columns = columns;
            this.rows = rows;
            this.charWidth = width / columns;
            this.charHeight = height / rows;
        }

        public void WriteTo(Bitmap target, int x, int y, string txt, Color color)
        {
            WriteTo(target, x, y, txt, (Color32)color);
        }

        public void WriteTo(Bitmap target, int x, int y, string txt, Color32 color)
        {
            int     xx = x;
            int     yy = y;
            Rect    r = new Rect();

            foreach (var c in txt)
            {
                if ((c < 32) || (c > 127))
                {
                    if (c == '\n')
                    {
                        xx = x;
                        yy += charHeight;
                    }
                    continue;
                }

                int cc = c - 32;
                r.x1 = charWidth * (cc % columns);
                r.y1 = charHeight * (cc / columns);
                r.x2 = r.x1 + charWidth;
                r.y2 = r.y1 + charHeight;
                target.BlitMask(xx, yy, this, r, color);
                xx += charWidth;
            }
        }

        public Rect GetTextRect(int x, int y, string txt)
        {
            int xx = x;
            int yy = y;
            Rect ret = new Rect(x, y, x, y + charHeight);

            foreach (var c in txt)
            {
                if (c < 32)
                {
                    if (c == '\n')
                    {
                        xx = x;
                        yy += charHeight;
                        ret.y2 = Mathf.Max(ret.y2, yy + charHeight);
                    }
                    continue;
                }

                xx += charWidth;

                ret.x2 = Mathf.Max(ret.x2, xx);
            }

            return ret;
        }
    }
}
