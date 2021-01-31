using SoftRender.Engine;

namespace SoftRender.Samples.RandomTriangles
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new RandomTriangles();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
