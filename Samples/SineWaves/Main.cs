using SoftRender.Engine;

namespace SoftRender.Samples.SineWaves
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new SineWaves();


            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
