using SoftRender.Engine;

namespace SoftRender.Samples.Text
{
    class Text : Application
    {
        Font font;

        public Text()
        {
            name = "Text Sample";
            clearColor = new Color(0.0f, 0.0f, 0.1f, 1.0f);
            windowResX = 1280;
            windowResY = 960;
        }

        protected override bool Initialize()
        {
            if (!base.Initialize()) return false;

            font = new Font("font.png");

            return true;
        }

        protected override void Loop()
        {
            screen.Clear(clearColor);

            screen.Write(50, 50, "Hello world!", font, Color.white);

            screen.Write(80, 100, "And here some multiline\ntext, sometimes it comes\nin handy!", font, Color.yellow);

            screen.Write(80, 150, "Finally, some background color...", font, Color.yellow, Color.red);
        }
    }
}
