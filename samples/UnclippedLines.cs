
namespace SoftRender.Samples
{
    class UnclippedLines : Application
    {
        public UnclippedLines()
        {
            name = "Unclipped Lines Sample";
            clearColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            windowResX = 1280;
            windowResY = 960;
        }

        protected override void Loop()
        {
            screen.Clear(clearColor);

            screen.DrawLine_Unclipped(new Vector2(20, 20), new Vector2(100, 20), new Color(1.0f, 0.0f, 0.0f, 1.0f));
            screen.DrawLine_Unclipped(new Vector2(20, 20), new Vector2(20, 120), new Color(1.0f, 1.0f, 0.0f, 1.0f));
            screen.DrawLine_Unclipped(new Vector2(50, 50), new Vector2(250, 250), new Color(0.0f, 1.0f, 0.0f, 1.0f));
            screen.DrawLine_Unclipped(new Vector2(100, 60), new Vector2(250, 140), new Color(0.0f, 1.0f, 1.0f, 1.0f));
            screen.DrawLine_Unclipped(new Vector2(80, 140), new Vector2(110, 300), new Color(1.0f, 0.0f, 1.0f, 1.0f));
        }
    }
}
