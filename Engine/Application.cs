﻿using System;
using SDL2;
using Mathlib;

namespace SoftRender.Engine
{
    public class Application
    {
        IntPtr  windowSurface;
        IntPtr  renderer;
        Bitmap  primarySurface;
        long    currentTimestamp;

        protected IntPtr    window;
        protected Bitmap    screen;

        protected Color     clearColor;
        protected bool      enableDepthBuffer;
        protected float     clearDepth;
        protected int       windowResX, windowResY;
        public    float     resScale;
        protected string    name;
        protected bool      exit;
        protected bool      writeFPS;
        protected Font      defaultFont;

        public float deltaTime;
        public float currentTime;
        public float loopTime;

        static public Application current;

        public int resX { get { return (int)(windowResX * resScale); } }
        public int resY { get { return (int)(windowResY * resScale); } }

        protected Application()
        {
            name = "Application";
            window = IntPtr.Zero;
            renderer = IntPtr.Zero;
            resScale = 1.0f;
            clearColor = new Color(1.0f, 0.0f, 1.0f, 1.0f);
            enableDepthBuffer = false;
            clearDepth = 1.0f;
            writeFPS = false;
            // Check if font exists
            if (System.IO.File.Exists("font.png"))
            {
                defaultFont = new Font("font.png");
            }
            windowResX = 640;
            windowResY = 480;
            current = this;
        }

        public bool Run()
        {
            if (!Startup())
            {
                return false;
            }

            if (!Initialize())
            {
                return false;
            }

            currentTimestamp = DateTime.Now.Ticks;
            deltaTime = 1.0f / 60.0f;
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
                        case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                            {
                                // Get mouse position
                                int x, y;
                                SDL.SDL_GetMouseState(out x, out y);
                                // Convert from window coordinates to screen buffer coordinates
                                float mx = (x / (float)windowResX) * resX;
                                float my = (y / (float)windowResY) * resY;
                                OnMouseDown(mx, my);
                            }
                            break;
                    }
                }

                long t0 = DateTime.Now.Ticks;
                Loop();
                long t1 = DateTime.Now.Ticks;

                loopTime = (t1 - t0) * 10e-7f;

                if ((writeFPS) && (defaultFont != null))
                {
                    float  fps = 1.0f / deltaTime;
                    string fpsText = string.Format("{0,7:###.0} FPS", fps);
                    screen.Write(0, 0, fpsText, defaultFont, Color32.white, Color32.black);
                }

                if (resScale == 1.0f)
                {
                    CopyToSurface(screen, windowSurface);
                }
                else
                {
                    // Scale up the screen to the primary surface
                    primarySurface.BlitScale(screen, 0, 0, resScale);
                    CopyToSurface(primarySurface, windowSurface);
                }

                SDL.SDL_UpdateWindowSurface(window);

                long t = DateTime.Now.Ticks;
                deltaTime = (float)((t - currentTimestamp) * 10e-7);
#if DEBUG
                if (deltaTime > 1) deltaTime = 1;
#endif
                currentTime = currentTime + deltaTime;
                currentTimestamp = t;
            }

            Cleanup();

            Shutdown();


            return true;
        }

        virtual protected bool Startup()
        {
            Random.InitState((int)DateTime.Now.Ticks);

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

            primarySurface = new Bitmap(windowResX, windowResY);
            screen = new Bitmap(resX, resY);
            if (enableDepthBuffer) screen.AttachDepthBuffer();

            return true;
        }

        void Shutdown()
        {
            SDL.SDL_Quit();
        }

        unsafe void CopyToSurface(Bitmap src, IntPtr dest)
        {
            var surfaceData = (SDL.SDL_Surface)System.Runtime.InteropServices.Marshal.PtrToStructure(dest, typeof(SDL.SDL_Surface));

            if (surfaceData.pitch == surfaceData.w * 4)
            {
                fixed (Color32* srcData = &src.data[0])
                {
                    Buffer.MemoryCopy(srcData, surfaceData.pixels.ToPointer(), src.width * src.height * 4, src.width * src.height * 4);
                }
            }
            else
            {
                var destData = surfaceData.pixels;
                fixed (Color32* srcData = &src.data[0])
                {
                    Color32* srcLine = srcData;

                    for (int y = 0; y < src.height; y++)
                    {
                        Buffer.MemoryCopy(srcLine, destData.ToPointer(), src.width * src.height * 4, src.width * 4);

                        srcLine = srcLine + src.width;
                        destData = destData + surfaceData.pitch;
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

        virtual protected void OnMouseDown(float x, float y)
        {

        }

        public Bitmap GetScreen() => screen;

        public Font GetDefaultFont() => defaultFont;

        public IntPtr GetWindowPtr() => window;
    }
}
