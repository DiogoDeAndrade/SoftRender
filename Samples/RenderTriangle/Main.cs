using SoftRender.Engine;

namespace SoftRender.Samples.RenderTriangle
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new RenderTriangle();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
