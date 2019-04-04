using System;
using System.Drawing;
using DrawDotNet.Interfaces;
using SDL2;

namespace DrawDotNet.Drawables
{
    public class Line : IDrawable
    {
        SDL.SDL_Point p1;
        SDL.SDL_Point p2;
        Color color;

        public Line(SDL.SDL_Point p1, SDL.SDL_Point p2, Color color): this(p1, p2)
        {
            this.color = color;
        }

        public Line(SDL.SDL_Point p1, SDL.SDL_Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        public void Dispose()
        {
        }

        public void Draw(IntPtr renderer, Color rendererColor)
        { 
            if (color != null) SDL.SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
            SDL.SDL_RenderDrawLine(renderer, p1.x, p1.y, p2.x, p2.y);
            SDL.SDL_SetRenderDrawColor(renderer, rendererColor.R, rendererColor.G, rendererColor.B, rendererColor.A);
        }

        public void Update()
        {

        }
    }
}
