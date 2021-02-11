using SoftRender.Engine;

namespace SoftRender.Samples.UnityApp.Fog
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new Fog();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
