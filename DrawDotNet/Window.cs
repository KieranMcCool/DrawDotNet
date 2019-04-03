﻿using System;
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

        Point location;
        string title;

        ConcurrentBag<IDrawable> entities;

        bool renderIsInit = false;

        FixedRateLooper renderThread;
        FixedRateLooper updateThread;
        FixedRateLooper eventThread;

        SDL.SDL_Rect backgroundArea;

        Color DrawingColor;
        Color BackgroundColor;


        #region constructors
        /* Constructors
         * ============ */

        public Window(string title, int width, int height) : 
        this(title, width, height, Color.Black) { }

        public Window(string title, int width, int height, Color backgroundColour)
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

            var tickRate = 60;

            renderThread = new FixedRateLooper("render-thread", tickRate, renderLoop);
            updateThread = new FixedRateLooper("update-thread", tickRate, new Action(() =>
                { foreach (var e in entities) e.Update(); }));
            eventThread = new FixedRateLooper("event-thread", -1, eventLoop, false);

            this.title = title;
        }
#endregion

        private void init()
        {
            IntPtr window;
            IntPtr renderer;

            window = SDL.SDL_CreateWindow("", 100, 100, Width, Height, SDL.SDL_WindowFlags.SDL_WINDOW_INPUT_FOCUS);
            renderer = SDL.SDL_CreateRenderer(window, 0, SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

            if (title != null) SDL.SDL_SetWindowTitle(WindowPtr, title);

            WindowPtr = window;
            RendererPtr = renderer;

            renderIsInit = true;
        }


        public void Show()
        {
            SDL.SDL_ShowWindow(WindowPtr);

            if (!renderIsInit) init();
            updateThread.Start();
            renderThread.Start();
            eventThread.StartSynchronous();
        }

        public void setPixel(int x, int y) 
        {
            SDL.SDL_RenderDrawPoint(RendererPtr, x, y);
        }

        public void Dispose()
        {
            renderThread.Cancel(); updateThread.Cancel(); eventThread.Cancel();
            SDL.SDL_DestroyRenderer(RendererPtr);
            SDL.SDL_DestroyWindow(WindowPtr);
        }

        public void addEntity(IDrawable entity)
        {
            entities.Add(entity);
        }

        private void eventLoop()
        {
            SDL.SDL_Event e;
            SDL.SDL_PollEvent(out e);

            var leftMouseClick = e.button.state == 1 && e.button.button == 1;

            var rightMouseClick = e.button.state == 1 && e.button.button == 3;
            var mouseLocation = new Point(e.button.x, e.button.y);
            var keyPressed = e.key.state == 1;
            string key;
            if (keyPressed)
            {
                key = SDL.SDL_GetKeyName(e.key.keysym.sym);
                Console.WriteLine(key);
            }


            if (leftMouseClick)
            {
                entities.Add(new Drawables.Rectangle(mouseLocation.X, mouseLocation.Y,
                    Utilities.Constants.RandomNumberGenerator.Next(10, 300), Utilities.Constants.RandomNumberGenerator.Next(10, 300), true));
            }

        }

        private void renderLoop()
        {
            Utilities.ColorHelpers.setRenderColour(RendererPtr, BackgroundColor);
            SDL.SDL_RenderFillRect(RendererPtr, ref backgroundArea);
            Utilities.ColorHelpers.setRenderColour(RendererPtr, DrawingColor);
            foreach (IDrawable d in entities) d.Draw(RendererPtr, DrawingColor);
            SDL.SDL_RenderPresent(RendererPtr);
        }

#region properties

        /* Properties
         * =========== */
        public Point Location
        {
            get => location;

            set
            {
                if (!location.Equals(value))
                {
                    this.location = value;
                    SDL.SDL_SetWindowPosition(WindowPtr, location.X, location.Y);
                }
            }
        }

        public String Title
        {
            get => title;
            set
            {
                if (!title.Equals(value))
                {
                    title = value;
                    SDL.SDL_SetWindowTitle(WindowPtr, title);
                }
            }
        }
        #endregion
    }
}
