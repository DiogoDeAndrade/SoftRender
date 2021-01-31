using SoftRender.Engine;

namespace SoftRender.Samples.Images
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new Images();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
