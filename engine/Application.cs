using System;
using SDL2;

namespace SoftRender
{
    class Application
    {
        IntPtr  window;
        IntPtr  windowSurface;
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
                while (SDL.SDL_PollEvent(out evt) != 0)
                {
                    switch (evt.type)
                    {
                        case SDL.SDL_EventType.SDL_QUIT:
                            exit = true;
                            break;
                        case SDL.SDL_EventType.SDL_KEYDOWN:
                            if ((evt.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE) && ((evt.key.keysym.mod & SDL.SDL_Keymod.KMOD_SHIFT) != 0))
                            {
                                exit = true;
                            }
                            break;
                    }
                }

                Loop();

                CopyToSurface(screen, primarySurface);

                SDL.SDL_BlitScaled(primarySurface, IntPtr.Zero, windowSurface, IntPtr.Zero);

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
                Debug.Log("Can't initialize SDL2");
                return false;
            }

            window = SDL.SDL_CreateWindow(name, SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, windowResX, windowResY, 0);
            if (window == IntPtr.Zero)
            {
                Debug.Log("Can't create window or renderer!");
                return false;
            }
            windowSurface = SDL.SDL_GetWindowSurface(window);
            renderer = SDL.SDL_CreateSoftwareRenderer(windowSurface);

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

            if (surfaceData.pitch == surfaceData.w * 4)
            {
                fixed (byte* srcData = &src.data[0].r)
                {
                    Buffer.MemoryCopy(srcData, surfaceData.pixels.ToPointer(), src.width * src.height * 4, src.width * src.height * 4);
                }
            }
            else
            {
                var destData = surfaceData.pixels;
                fixed (byte* srcData = &src.data[0].r)
                {
                    byte* srcLine = srcData;
                    
                    for (int y = 0; y < src.height; y++)
                    {
                        Buffer.MemoryCopy(srcLine, destData.ToPointer(), src.width * src.height * 4, src.width * 4);

                        srcLine += src.width * 4;
                        destData += surfaceData.pitch;
                    }
                }
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
