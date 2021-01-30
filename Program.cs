using System;

namespace SoftRender
{
    class Program
    {
        static void Main(string[] args)
        {
//            Application app = new Samples.ClearScreen();
//            Application app = new Samples.SineWaves();
//            Application app = new Samples.UnclippedLines();
            Application app = new Samples.ClippedLines();


            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
