using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using DrawDotNet.Drawables;
using DrawDotNet.Interfaces;
using DrawDotNet.Utilities;
using SDL2;

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

        FixedRateLooper SdlThread;
        FixedRateLooper UpdateThread;

        SDL.SDL_Rect backgroundArea;

        Color DrawingColor;
        Color BackgroundColor;

        #region constructors
        /* Constructors
         * ============ */

        public Window(string title, int width, int height):
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

            var tickRate = -1;
            bool printLog = false;

            SdlThread = new FixedRateLooper("render-thread", tickRate, SDLLoop, printLog);
            UpdateThread = new FixedRateLooper("update-thread", tickRate, UpdateLoop, printLog);

            this.title = title;
        }
        #endregion

        /// <summary>
        /// Performas setup for window. Thisis called when the window is first shown.
        /// </summary>
        private void InitialiseWindow()
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

        /// <summary>
        /// Shows the window and starts its event loops if their not already started.
        /// </summary>
        public void Show()
        {
            SDL.SDL_ShowWindow(WindowPtr);

            if (!renderIsInit) InitialiseWindow();
            UpdateThread.Start();
            SdlThread.StartSynchronous();
        }

        /// <summary>
        ///  Draws a single pixel in the renderer's current colour.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPixel(int x, int y)
        {
            SDL.SDL_RenderDrawPoint(RendererPtr, x, y);
        }

        public void Dispose()
        {
            SdlThread.Cancel();
            UpdateThread.Cancel();
            SDL.SDL_DestroyRenderer(RendererPtr);
            SDL.SDL_DestroyWindow(WindowPtr);
        }

        public void AddEntity(IDrawable entity)
        {
            entities.Add(entity);
        }

        /// <summary>
        /// This is a loop which runs continuously and updates the entities.
        /// </summary>
        private void UpdateLoop()
        {
            foreach (var e in entities) e.Update();
        }

        /// <summary>
        /// This thread processes both SDL Events and render calls. All SDL Activity must be invoked from somewhere in this loop/
        /// </summary>
        private void SDLLoop()
        {
            SDL.SDL_Event e;

            while (SDL.SDL_PollEvent(out e) != 0)
            {
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
                    var rng = Utilities.Constants.RandomNumberGenerator;
                    entities.Add(new Drawables.Rectangle(mouseLocation.X, mouseLocation.Y,
                        rng.Next(10, 300), rng.Next(10, 300), true));
                }

                var quit = e.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE;
                if (quit) Dispose();
            }

            RenderLoop();
        }

        /// <summary>
        /// Iterates over entities and calls their draw functions and renders them. Called from the SDLThread.
        /// </summary>
        private void RenderLoop()
        {
            SDL.SDL_RenderClear(RendererPtr);
            Utilities.ColorHelpers.setRenderColour(RendererPtr, BackgroundColor);
            SDL.SDL_RenderFillRect(RendererPtr, ref backgroundArea);
            Utilities.ColorHelpers.setRenderColour(RendererPtr, DrawingColor);
            foreach (IDrawable d in entities) d.Draw(RendererPtr, DrawingColor);
            SDL.SDL_RenderPresent(RendererPtr);
            SDLLoop();
        }

        #region properties

        /* Properties
         * =========== */

        public void Focus()
        {
            SDL.SDL_RestoreWindow(WindowPtr);
            SDL.SDL_RaiseWindow(WindowPtr);
        }

        public void Minimise()
        {
            SDL.SDL_MinimizeWindow(WindowPtr);
        }

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