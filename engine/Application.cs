using System;
using SDL2;

namespace SoftRender
{
    class Application
    {
        IntPtr  window;
        IntPtr  renderer;
        IntPtr  primarySurface;
        long    currentTimestamp;

        protected Bitmap  screen;

        protected Color   clearColor;
        protected int     resX, resY;
        protected int     windowResX, windowResY;
        protected string  name;
        protected bool    exit;

        public static float deltaTime;
        public static float currentTime;        

        protected Application()
        {
            name = "Application";
            window = IntPtr.Zero;
            renderer = IntPtr.Zero;
            primarySurface = IntPtr.Zero;
            resX = windowResX = 640;
            resY = windowResY = 480;
            clearColor = new Color(1.0f, 0.0f, 1.0f, 1.0f);
        }

        public bool Run()
        {
            if (!Initialize())
            {
                return false;
            }

            if (!Startup())
            {
                return false;
            }

            currentTimestamp = DateTime.Now.Ticks;
            deltaTime = 60.0f / 1000.0f;
            currentTime = 0;

            exit = false;
            while (!exit)
            {

                SDL.SDL_Event evt;
                SDL.SDL_PollEvent(out evt);
                switch (evt.type)
                {
                    case SDL.SDL_EventType.SDL_QUIT:
                        exit = true;
                        break;
                    case SDL.SDL_EventType.SDL_KEYDOWN:
                        if (evt.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE)
                        {
                            exit = true;
                        }
                        break;
                }

                Loop();

                CopyToSurface(screen, primarySurface);

                SDL.SDL_BlitScaled(primarySurface, IntPtr.Zero, SDL.SDL_GetWindowSurface(window), IntPtr.Zero);

                SDL.SDL_UpdateWindowSurface(window);

                long t = DateTime.Now.Ticks;
                deltaTime = (t - currentTimestamp) *10e-7f;
                currentTime += deltaTime;
                currentTimestamp = t;
            }

            Cleanup();

            Shutdown();


            return true;
        }

        bool Startup()
        {
            if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) != 0)
            {
                Console.WriteLine("Can't initialize SDL2");
                return false;
            }

            if (SDL.SDL_CreateWindowAndRenderer(windowResX, windowResY, 0, out window, out renderer) != 0)
            {
                Console.WriteLine("Can't create window or renderer!");
                return false;
            }
            SDL.SDL_SetWindowTitle(window, name);

            primarySurface = SDL.SDL_CreateRGBSurface(0, resX, resY, 32, 0x000000FF, 0x0000FF00, 0x00FF0000, 0xFF000000);
            screen = new Bitmap(resX, resY);

            return true;
        }

        void Shutdown()
        {
            SDL.SDL_FreeSurface(primarySurface);
            SDL.SDL_Quit();
        }

        unsafe void CopyToSurface(Bitmap src, IntPtr dest)
        {
            var surfaceData = (SDL.SDL_Surface)System.Runtime.InteropServices.Marshal.PtrToStructure(dest, typeof(SDL.SDL_Surface));

            fixed (byte* srcData = &src.data[0].r)
            {
                Buffer.MemoryCopy(srcData, surfaceData.pixels.ToPointer(), src.width * src.height * 4, src.width * src.height * 4);
            }
        }

        virtual protected bool Initialize()
        {
            return true;
        }

        virtual protected void Loop()
        {
        }

        virtual protected void Cleanup()
        {

        }
    }
}
