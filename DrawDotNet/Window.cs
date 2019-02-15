using System;
using SDL2;

namespace DrawDotNet
{
    public class Window : IDisposable
    {
        IntPtr WindowPtr;
        IntPtr RendererPtr;

        public Window(int width, int height)
        {
            if (SDL.SDL_WasInit(SDL.SDL_INIT_VIDEO) == 0)
                SDL.SDL_Init(SDL.SDL_INIT_VIDEO);

            IntPtr window;
            IntPtr renderer;

            SDL.SDL_CreateWindowAndRenderer(width, height,
                SDL.SDL_WindowFlags.SDL_WINDOW_INPUT_FOCUS, out window, out renderer);

            WindowPtr = window;
            RendererPtr = renderer;
        }

        public void Show()
        {
            SDL.SDL_ShowWindow(WindowPtr);
        }

        public void Dispose()
        {
            SDL.SDL_DestroyRenderer(RendererPtr);
            SDL.SDL_DestroyWindow(WindowPtr);
        }
    }
}
