using System;

namespace SoftRender
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new Samples.ClearScreen();

            if (!app.Run())
            {
                Debug.Log("Failed to run application!");
            }
        }
    }
}
