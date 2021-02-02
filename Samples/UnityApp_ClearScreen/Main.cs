using SoftRender.Engine;

namespace SoftRender.Samples.UnityApp.ClearScreen
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new ClearScreen();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
