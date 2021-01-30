using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRender
{
    static public class Random
    {
        static System.Random randomGenerator;

        static public void InitState(int seed)
        {
            randomGenerator = new System.Random(seed);
        }

        static public int Range(int min, int max)
        {
            if (min == max) return min;

            int val = randomGenerator.Next();

            return min + val % (max - min);
        }

        static public float Range(float min, float max)
        {
            float val = (float)randomGenerator.NextDouble();

            return min + val * (max - min);
        }

        static public Color color
        {
            get { return new Color(Range(0.0f, 1.0f), Range(0.0f, 1.0f), Range(0.0f, 1.0f), Range(0.0f, 1.0f)); }
        }
        static public Color opaqueColor
        {
            get { return new Color(Range(0.0f, 1.0f), Range(0.0f, 1.0f), Range(0.0f, 1.0f), 1.0f); }
        }

        static public Color32 color32
        {
            get { return (Color32)color; }
        }
        static public Color32 opaqueColor32
        {
            get { return (Color32)opaqueColor; }
        }
    }
}
