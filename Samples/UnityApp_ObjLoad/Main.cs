using SoftRender.Engine;

namespace SoftRender.Samples.UnityApp.ObjLoad
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new ObjLoad();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
