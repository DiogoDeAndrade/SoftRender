using SoftRender.Engine;

namespace SoftRender.Samples.UnityApp.Textures
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new Textures();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
