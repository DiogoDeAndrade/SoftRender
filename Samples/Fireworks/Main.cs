using SoftRender.Engine;

namespace SoftRender.Samples.Fireworks
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new Fireworks();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
