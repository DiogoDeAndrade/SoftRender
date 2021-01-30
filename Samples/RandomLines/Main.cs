using SoftRender.Engine;

namespace SoftRender.Samples.RandomLines
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new RandomLines();


            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
