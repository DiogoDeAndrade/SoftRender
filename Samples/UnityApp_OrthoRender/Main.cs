using SoftRender.Engine;

namespace SoftRender.Samples.UnityApp.OrthoRender
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new OrthoRender();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
