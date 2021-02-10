using SoftRender.Engine;

namespace SoftRender.Samples.UnityApp.VertexLighting
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new VertexLighting();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
