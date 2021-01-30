using System;

namespace SoftRender
{
    class Program
    {
        static void Main(string[] args)
        {
//            Application app = new Samples.ClearScreen();
            Application app = new Samples.SineWaves();
            

            if (!app.Run())
            {
                Console.WriteLine("Failed to run application!");
            }
        }
    }
}
