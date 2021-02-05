
using SoftRender.Engine;

namespace SoftRender.Samples.SineWaves
{
    class SineWaves : Application
    {
        struct SineWaveStruct
        {
            public Vector2 direction;
            public float   amplitude;
            public float   frequency;
            public float   speed;
        }
        SineWaveStruct[]  sines;

        public SineWaves()
        {
            name = "Sine Waves Sample";
            resScale = 0.125f;

            sines = new SineWaveStruct[]
            {
                new SineWaveStruct { direction = new Vector2(1,0), amplitude = 0.35f, frequency = 0.1f, speed = 0.1f},
                new SineWaveStruct { direction = new Vector2(0,1), amplitude = 0.35f, frequency = 0.15f, speed = 0.2f },
                new SineWaveStruct { direction = new Vector2(-0.7f, 0.34f), amplitude = 0.20f, frequency = 0.25f, speed = 0.4f },
                new SineWaveStruct { direction = new Vector2(0.5f, -0.6f), amplitude = 0.10f , frequency = 0.4f, speed = 0.8f }
            };
        }

        override protected void Loop()
        {
            float amplitude = 0.5f / sines.Length;

            for (int y = 0; y < resY; y++)
            {
                for (int x = 0; x < resX; x++)
                {
                    float c = 0;

                    for (int s = 0; s < sines.Length; s++)
                    {
                        c = c + Mathf.Clamp01(SineWave(sines[s].direction, sines[s].amplitude, sines[s].frequency, x, y, sines[s].speed));
                    }

                    byte c32 = (byte)(c * 255.0f);

                    screen.data[x + y * resX].Set(c32, c32, c32, 255);
                }
            }
        }

        float SineWave(Vector2 direction, float amplitude, float frequency, float x, float y, float speed)
        {
            float d = (direction.x * x + direction.y * y);

            return 0.5f * amplitude * (Mathf.Sin(d * frequency + Time.time * speed) + 1.0f);
        }
    }
}
