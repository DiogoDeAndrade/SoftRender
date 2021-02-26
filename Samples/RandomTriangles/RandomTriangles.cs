using SoftRender.Engine;
using Mathlib;

namespace SoftRender.Samples.RandomTriangles
{
    class RandomTriangles : Application
    {
        int         nTriangles;
        double      accumRenderTriTime;

        public RandomTriangles()
        {
            name = "Random Triangles Sample";
            clearColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            windowResX = 1280;
            windowResY = 960;
            resScale = 0.25f;
            writeFPS = true;
            nTriangles = 0;
            accumRenderTriTime = 0;
        }

        protected override void Loop()
        {
            double t0 = Time.tickCount;

            int trianglesPerFrame = 1000;
            for (int i = 0; i < trianglesPerFrame; i++)
            {
                Vector2 p1 = new Vector2(Random.Range(-100.0f, resX + 100.0f), Random.Range(-100.0f, resY + 100.0f));
                Vector2 p2 = p1 + new Vector2(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
                Vector2 p3 = p1 + new Vector2(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));

                Color color = Random.opaqueColor;

                screen.DrawTriangle(p1, p2, p3, color);
            }
            nTriangles += trianglesPerFrame;

            accumRenderTriTime += (Time.tickCount - t0);

            double timePerTri = (accumRenderTriTime * 10e-4) / nTriangles;

            screen.Write(0, 10, $"Avg Render Triangle Time = {timePerTri,6:F4} ms/tri", defaultFont, Color.white, Color.black);
        }
    }
}
