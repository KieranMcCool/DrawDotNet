using System;
using System.Drawing;
using SDL2;
using DrawDotNet.Interfaces;
using DrawDotNet.Utilities;

namespace DrawDotNet.Drawables
{
    public class Rectangle : IDrawable
    {
        SDL.SDL_Rect rectangle;
        Color color;
        bool fill;
        public int j;

        static readonly Random r = Constants.RandomNumberGenerator;

        public Rectangle(int x, int y, int w, int h, bool fill)
        {
            rectangle = new SDL.SDL_Rect() { x = x, y = y, w = w, h = h };
            this.fill = fill;
        }

        public Rectangle(int x, int y, int w, int h, bool fill, Color c)
            :this(x,y,w,h, fill)
        {
            color = c;
        }

        public void Dispose()
        {
        }

        public void Draw(IntPtr renderer, Color rendererColor)
        {
            if (color != null) 
                Utilities.ColorHelpers.setRenderColour(renderer, color);

            if (fill) SDL.SDL_RenderFillRect(renderer, ref rectangle);
            else SDL.SDL_RenderDrawRect(renderer, ref rectangle);

            if (color != null)
                Utilities.ColorHelpers.setRenderColour(renderer, rendererColor);
        }

        public void Update()
        {
        }
    }
}
