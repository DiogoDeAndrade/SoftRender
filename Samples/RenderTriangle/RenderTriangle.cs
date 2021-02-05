using SoftRender.Engine;

namespace SoftRender.Samples.RenderTriangle
{
    class RenderTriangle : Application
    {
        public RenderTriangle()
        {
            name = "Triangle Render Sample";
            clearColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            windowResX = 1280;
            windowResY = 960;
        }

        protected override void Loop()
        {
            screen.Clear(clearColor);

            screen.DrawTriangle(new Vector2(320, 50), new Vector2(160, 160), new Vector2(460, 250), new Color(1.0f, 1.0f, 0.0f, 1.0f));
            screen.DrawTriangle(new Vector2(620, 50), new Vector2(460, 160), new Vector2(760, 250), new Color(1.0f, 0.0f, 0.0f, 1.0f));
            screen.DrawTriangle(new Vector2(-120, 50), new Vector2(-360, 160), new Vector2(60, 250), new Color(0.0f, 1.0f, 0.0f, 1.0f));
            screen.DrawTriangle(new Vector2(320, -150), new Vector2(160, -60), new Vector2(460, 50), new Color(1.0f, 0.0f, 1.0f, 1.0f));
            screen.DrawTriangle(new Vector2(320, 450), new Vector2(160, 560), new Vector2(460, 650), new Color(0.0f, 1.0f, 1.0f, 1.0f));

            screen.DrawTriangle(new Vector2(320, 250), new Vector2(160, 470), new Vector2(460, 400), new Color(0.0f, 1.0f, 0.0f, 1.0f));
            screen.DrawTriangle(new Vector2(50, 300), new Vector2(100, 470), new Vector2(200, 400), new Color(0.0f, 1.0f, 1.0f, 1.0f));
            screen.DrawTriangle(new Vector2(250, 40), new Vector2(50, 100), new Vector2(100, 150), new Color(1.0f, 0.0f, 1.0f, 1.0f));
            screen.DrawTriangle(new Vector2(500, 40), new Vector2(550, 40), new Vector2(525, 80), new Color(1.0f, 1.0f, 0.0f, 1.0f));
            screen.DrawTriangle(new Vector2(530, 300), new Vector2(550, 350), new Vector2(525, 350), new Color(1.0f, 1.0f, 1.0f, 1.0f));

            Color32[] colors = new Color32[] { new Color32(255,0,0), new Color32(255, 255, 0), new Color32(0,255,0), new Color32(0, 255, 255) };

            int     nSides = 7;
            float   angleDiv = Mathf.PI * 2.0f / nSides;
            Vector2 center = new Vector2(500, 400);
            float   radius = 25.0f;
            
            for (int i = 0; i < nSides; i = i + 1)
            {
                Vector2 p1 = new Vector2(center.x + radius * Mathf.Cos(angleDiv * i), center.y + radius * Mathf.Sin(angleDiv * i));
                Vector2 p2 = new Vector2(center.x + radius * Mathf.Cos(angleDiv * (i + 1)), center.y + radius * Mathf.Sin(angleDiv * (i + 1)));

                screen.DrawTriangle(center, p1, p2, colors[i % colors.Length]);
            }
        }
    }
}
