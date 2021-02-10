using System.Collections.Generic;
using SoftRender.Engine;

namespace SoftRender.UnityApp
{
    public abstract class Renderer : Behaviour
    {
        public static List<Renderer>  allRenderables = new List<Renderer>();
        public static Renderer        current;

        public Renderer() : base()
        {
            allRenderables.Add(this);
        }

        ~Renderer()
        {
            allRenderables.Remove(this);
        }

        abstract public void Render();
    }
}
