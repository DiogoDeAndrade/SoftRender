using SoftRender.Engine;

namespace SoftRender.Samples.UnityApp.DepthBuffer
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new DepthBuffer();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
