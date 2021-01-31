using SoftRender.Engine;

namespace SoftRender.Samples.Text
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new Text();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
