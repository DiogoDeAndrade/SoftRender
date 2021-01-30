using SoftRender.Engine;

namespace SoftRender.Samples.ClippedLines
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new ClippedLines();


            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
