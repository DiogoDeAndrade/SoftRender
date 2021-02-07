using SoftRender.Engine;

namespace SoftRender.Samples.UnityApp.BlendOps
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new BlendOps();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
