using SoftRender.Engine;
using Mathlib;

namespace SoftRender.Samples.Images
{
    class Images : Application
    {
        Bitmap image;
        float  animTimer;
        int    frameIndex;
        float  x;

        const float animSpeed = 1.0f;

        public Images()
        {
            name = "Images Sample";
            clearColor = new Color(0.0f, 0.0f, 0.2f, 1.0f);
            windowResX = 1280;
            windowResY = 960;
            resScale = 0.125f;
        }

        protected override bool Initialize()
        {
            if (!base.Initialize()) return false;

            image = new Bitmap("char_hero.png");

            return true;
        }

        protected override void Loop()
        {
            screen.Clear(clearColor);

            animTimer += Time.deltaTime;
            if (animTimer > animSpeed)
            {
                frameIndex = (frameIndex + 1) % 4;
                animTimer -= animSpeed;
            }

            var rect = image.GetFrameRect(4, 11, 12 + frameIndex);

            screen.BlitWithAlphablend((int)x, 0, image, rect);
            x += Time.deltaTime * 5;
            if (x > resX) x = -rect.width;
        }
    }
}
