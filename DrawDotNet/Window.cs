using System;
using SDL2;

namespace DrawDotNet
{
    public class Window
    {
        public Window()
        {
            if (SDL.SDL_WasInit(SDL.SDL_INIT_VIDEO) == 0)
            {
                SDL.SDL_Init(SDL.SDL_INIT_VIDEO);
            }
        }
    }
}
