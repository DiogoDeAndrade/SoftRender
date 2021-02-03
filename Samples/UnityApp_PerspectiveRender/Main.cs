using SoftRender.Engine;

namespace SoftRender.Samples.UnityApp.PerspectiveRender
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new PerspectiveRender();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
