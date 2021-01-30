
namespace SoftRender.Samples
{
    class ClippedLines : Application
    {
        public ClippedLines()
        {
            name = "Clipped Lines Sample";
            clearColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            windowResX = 1280;
            windowResY = 960;
        }

        protected override void Loop()
        {
            screen.Clear(clearColor);

            screen.DrawLine(new Vector2(20, 20), new Vector2(100, 20), new Color(1.0f, 0.0f, 0.0f, 1.0f));
            screen.DrawLine(new Vector2(20, 20), new Vector2(20, 120), new Color(1.0f, 1.0f, 0.0f, 1.0f));
            screen.DrawLine(new Vector2(50, 50), new Vector2(250, 250), new Color(0.0f, 1.0f, 0.0f, 1.0f));
            screen.DrawLine(new Vector2(100, 60), new Vector2(250, 140), new Color(0.0f, 1.0f, 1.0f, 1.0f));
            screen.DrawLine(new Vector2(80, 140), new Vector2(110, 300), new Color(1.0f, 0.0f, 1.0f, 1.0f));

            screen.DrawLine(new Vector2(580, 140), new Vector2(800, 300), new Color(1.0f, 1.0f, 1.0f, 1.0f));
            screen.DrawLine(new Vector2(580, 140), new Vector2(470, -300), new Color(1.0f, 1.0f, 1.0f, 1.0f));
            screen.DrawLine(new Vector2(580, 140), new Vector2(-100, 100), new Color(1.0f, 1.0f, 1.0f, 1.0f));
            screen.DrawLine(new Vector2(580, 140), new Vector2(100, 600), new Color(1.0f, 1.0f, 1.0f, 1.0f));

            screen.DrawLine(new Vector2(790, 310), new Vector2(100, 300), new Color(0.0f, 0.0f, 1.0f, 1.0f));
            screen.DrawLine(new Vector2(460, -310), new Vector2(100, 300), new Color(0.0f, 0.0f, 1.0f, 1.0f));
            screen.DrawLine(new Vector2(-110, 110), new Vector2(100, 300), new Color(0.0f, 0.0f, 1.0f, 1.0f));
            screen.DrawLine(new Vector2(90, 610), new Vector2(100, 300), new Color(0.0f, 0.0f, 1.0f, 1.0f));

            screen.DrawLine(new Vector2(-100, 120), new Vector2(750, 300), new Color(0.5f, 0.5f, 0.5f, 1.0f));
            screen.DrawLine(new Vector2(300, -45), new Vector2(390, 540), new Color(0.5f, 0.5f, 0.5f, 1.0f));
            screen.DrawLine(new Vector2(500, -45), new Vector2(700, 100), new Color(0.5f, 0.5f, 0.5f, 1.0f));

            screen.DrawLine(new Vector2(-100, 100), new Vector2(10, -50), new Color(1.0f, 0.0f, 0.0f, 1.0f));
        }
    }
}
