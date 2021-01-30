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
    }
}
