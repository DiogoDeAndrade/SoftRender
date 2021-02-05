using SoftRender.Engine;

namespace SoftRender.Samples.RandomLines
{
    class RandomLines : Application
    {
        public RandomLines()
        {
            name = "Random Lines Sample";
            clearColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            windowResX = 1280;
            windowResY = 960;
        }

        protected override void Loop()
        {
            int linesPerFrame = 10;
            for (int i = 0; i < linesPerFrame; i++)
            {
                Vector2 p1 = new Vector2(Random.Range(-100.0f, resX + 100.0f), Random.Range(-100.0f, resY + 100.0f));
                Vector2 p2 = new Vector2(Random.Range(-100.0f, resX + 100.0f), Random.Range(-100.0f, resY + 100.0f));

                Color color = Random.opaqueColor;

                screen.DrawLine(p1, p2, color);
            }
        }
    }
}
