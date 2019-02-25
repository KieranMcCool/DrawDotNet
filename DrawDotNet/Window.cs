using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using SDL2;
using DrawDotNet.Interfaces;
using DrawDotNet.Drawables;
using DrawDotNet.Utilities;

namespace DrawDotNet
{
    public class Window : IDisposable
    {
        IntPtr WindowPtr;
        IntPtr RendererPtr;

        int Width;
        int Height;

        ConcurrentBag<IDrawable> entities;

        bool renderIsInit = false;

        FixedRateLooper renderThread;
        FixedRateLooper updateThread;

        SDL.SDL_Rect backgroundArea;

        Color DrawingColor;
        Color BackgroundColor;

        public Window(int width, int height) : this(width, height, Color.Black)
        {
        }

        public Window(int width, int height, Color backgroundColour)
        {
            Width = width;
            Height = height;

            entities = new ConcurrentBag<IDrawable>();
            DrawingColor = Color.Yellow;

            if (SDL.SDL_WasInit(SDL.SDL_INIT_VIDEO) == 0)
                SDL.SDL_Init(SDL.SDL_INIT_VIDEO);
            
            BackgroundColor = backgroundColour;
            backgroundArea = new SDL.SDL_Rect()
            {
                y = 0,
                x = 0,
                w = Width,
                h = Height
            };

            renderThread = new FixedRateLooper(-1, renderLoop);
            updateThread = new FixedRateLooper(1, new Action(() =>
                { foreach (var e in entities) e.Update(); }));
        }


        private void init()
        {
            IntPtr window;
            IntPtr renderer;

            window = SDL.SDL_CreateWindow("", 0, 0, Width, Height, SDL.SDL_WindowFlags.SDL_WINDOW_INPUT_FOCUS);
            renderer = SDL.SDL_CreateRenderer(window, 0, SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

            WindowPtr = window;
            RendererPtr = renderer;

            renderIsInit = true;
        }


        public void Show()
        {
            SDL.SDL_ShowWindow(WindowPtr);
            renderThread.Start();
            updateThread.Start();
        }

        public void Dispose()
        {
            renderThread.Cancel(); updateThread.Cancel();
            renderThread.Join(); updateThread.Join();
            SDL.SDL_DestroyRenderer(RendererPtr);
            SDL.SDL_DestroyWindow(WindowPtr);
        }

        public void addEntity(IDrawable entity)
        {
            entities.Add(entity);
        }

        private void renderLoop()
        {
            if (!renderIsInit) init();
            Utilities.ColorHelpers.setRenderColour(RendererPtr, BackgroundColor);
            SDL.SDL_RenderFillRect(RendererPtr, ref backgroundArea);
            Utilities.ColorHelpers.setRenderColour(RendererPtr, DrawingColor);
            foreach (IDrawable d in entities) d.Draw(RendererPtr, DrawingColor);
            SDL.SDL_RenderPresent(RendererPtr);
        }
    }
}
