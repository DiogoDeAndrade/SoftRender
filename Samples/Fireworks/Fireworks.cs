
using System.Collections.Generic;
using SoftRender.Engine;

namespace SoftRender.Samples.Fireworks
{
    class Fireworks : Application
    {
        class Firework
        {
            class Particle
            {
                public float    totalLife;
                public float    life;
                public Vector2  position;
                public Vector2  oldPos;
                public Vector2  velocity;
                public Color    color;
            }

            public float    life;
            public Vector2  emitterPos;
            public Vector2  particleLife;
            public Vector2  particleSpeed;
            public Color    particleColor;
            public float    particleDrag;
            public Vector2  gravity;

            List<Particle> particles;

            public Firework()
            {
                particles = new List<Particle>();
            }

            public void Update()
            {
                life -= Time.deltaTime;

                foreach (var particle in particles)
                {
                    particle.oldPos = particle.position;
                    particle.position = particle.position + particle.velocity * Time.deltaTime;
                    particle.velocity = (1.0f - particleDrag) * particle.velocity + gravity * Time.deltaTime; ;
                    particle.color = (particle.life / particle.totalLife) * particleColor;
                    particle.color.a = 1.0f;
                    particle.life -= Time.deltaTime;
                }

                particles.RemoveAll((p) => p.life <= 0);

                // Diminish drag over time
                particleDrag = Mathf.Max(0, particleDrag - 0.005f * Time.deltaTime);
            }

            public void SpawnParticles(int nParticles)
            {
                for (int i = 0; i < nParticles; i++)
                {
                    Particle particle = new Particle
                    {
                        totalLife = Random.Range(particleLife.x, particleLife.y),
                        position = emitterPos,
                        velocity = Random.normalizedVector2 * Random.Range(particleSpeed.x, particleSpeed.y),
                        color = particleColor
                    };
                    particle.life = particle.totalLife;
                    particle.oldPos = particle.position;

                    particles.Add(particle);
                }
            }

            public void Render(Bitmap target)
            {
                foreach (var particle in particles)
                {
                    target.DrawLine(particle.position, particle.oldPos, particle.color);
                }
            }

            public bool isAlive { get { return (life > 0) || (particles.Count > 0); } }
        };

        List<Firework>  fireworks;
        Color[]         colors =
        {
            new Color(1,0,0,1),
            new Color(1,1,0,1),
            new Color(1,0,1,1),
            new Color(0,1,1,1),
            new Color(0,1,0,1)
        };

        public Fireworks()
        {
            name = "Fireworks Sample";
            clearColor = new Color(0, 0, 0, 1);
            windowResX = 1280;
            windowResY = 960;
        }

        protected override bool Initialize()
        {
            if (!base.Initialize()) return false;

            fireworks = new List<Firework>();

            return true;
        }

        protected override void Loop()
        {
            foreach (var fw in fireworks)
            {
                fw.Update();
            }

            fireworks.RemoveAll((fw) => !fw.isAlive);

            screen.Clear(clearColor);
            foreach (var fw in fireworks)
            {
                fw.Render(screen);
            }
        }

        void SpawnPS(float x, float y)
        {
            Firework fw = new Firework();
            fw.life = 5.0f;
            fw.emitterPos = new Vector2(x, y);
            fw.particleLife = new Vector2(3.0f, 10.0f);
            fw.particleSpeed = new Vector2(20.0f, 60.0f);
            fw.particleColor = colors[Random.Range(0, colors.Length)];
            fw.particleDrag = 0.02f;
            fw.gravity = new Vector2(0, 10.0f);
            fw.SpawnParticles(1200);

            fireworks.Add(fw);
        }

        override protected void OnMouseDown(float x, float y)
        {
            SpawnPS(x, y);
        }

    }
}
