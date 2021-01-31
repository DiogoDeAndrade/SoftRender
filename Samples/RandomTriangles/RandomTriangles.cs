using SoftRender.Engine;

namespace SoftRender.Samples.RandomTriangles
{
    class RandomTriangles : Application
    {
        public RandomTriangles()
        {
            name = "Random Triangles Sample";
            clearColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            windowResX = 1280;
            windowResY = 960;
        }

        protected override void Loop()
        {
            int trianglesPerFrame = 1000;
            for (int i = 0; i < trianglesPerFrame; i++)
            {
                Vector2 p1 = new Vector2(Random.Range(-100.0f, resX + 100.0f), Random.Range(-100.0f, resY + 100.0f));
                Vector2 p2 = p1 + new Vector2(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
                Vector2 p3 = p1 + new Vector2(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));

                Color color = Random.opaqueColor;

                screen.DrawTriangle(p1, p2, p3, color);
            }
        }
    }
}
