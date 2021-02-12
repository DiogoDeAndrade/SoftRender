using SoftRender.Engine;

namespace SoftRender.Samples.UnityApp.NormalMapping
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new NormalMapping();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
