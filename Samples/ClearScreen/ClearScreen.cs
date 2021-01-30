using SoftRender.Engine;

namespace SoftRender.Samples.ClearScreen
{
    class ClearScreen : Application
    {
        public ClearScreen()
        {
            name = "Clear Screen Sample";
        }

        protected override void Loop()
        {
            screen.Clear(clearColor);
        }
    }
}
