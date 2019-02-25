using System;
using System.Drawing;
using SDL2;
using DrawDotNet.Interfaces;

namespace DrawDotNet.Drawables
{
    public class Rectangle : IDrawable
    {
        SDL.SDL_Rect rectangle;
        Color color;
        bool fill;
        public int j;

        static Random r = new Random();

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
            j++;
            var direction = r.Next(4);
            switch(direction)
            {
                case 0:

                    rectangle.x += r.Next(-5, 5);
                    break;
                case 1:
                    rectangle.y += r.Next(-5, 5);
                    break;
                case 2:
                    rectangle.w += r.Next(-5, 5);
                    break;
                case 3:
                    rectangle.h += r.Next(-5, 5);
                    break;
            }
        }
    }
}
