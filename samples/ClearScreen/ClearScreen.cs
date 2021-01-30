
namespace SoftRender.Samples
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
