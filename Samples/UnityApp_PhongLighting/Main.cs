using SoftRender.Engine;

namespace SoftRender.Samples.UnityApp.PhongLighting
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new PhongLighting();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
