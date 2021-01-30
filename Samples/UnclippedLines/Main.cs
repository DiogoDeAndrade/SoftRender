using SoftRender.Engine;

namespace SoftRender.Samples.UnclippedLines
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new UnclippedLines();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
