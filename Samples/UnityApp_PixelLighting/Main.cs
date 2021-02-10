using SoftRender.Engine;

namespace SoftRender.Samples.UnityApp.PixelLighting
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new PixelLighting();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
