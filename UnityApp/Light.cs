using SoftRender.Engine;
using System.Collections.Generic;

namespace SoftRender.UnityApp
{
    public enum LightType { Spot, Directional, Point, Rectangle, Disc };

    public class Light : Component
    {
        public Color        color = Color.white;
        public float        intensity = 1.0f;
        public LightType    type = LightType.Point;

        public static List<Light> allLights = new List<Light>();

        public Light() : base()
        {
            allLights.Add(this);
        }

        ~Light()
        {
            allLights.Remove(this);
        }
    }
}
