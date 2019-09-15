using System;
using System.Drawing;
using System.Collections.Concurrent;
using DrawDotNet.Interfaces;
using System.Linq;
using SDL2;

namespace DrawDotNet.Drawables
{
    public class Dot : IDrawable
    {
        int x;
        int y;
        Color c;
        float age = -10;
        private readonly float ageinc = 1e-1f;
        private static readonly int trailLen = 100;
        private int trailPos = 0;
        SDL.SDL_Point[] trail = new SDL.SDL_Point[trailLen];


        public Dot(int x, int y, Color c): this(x, y)
        {
            this.c = c;
        }

        public Dot(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Dispose()
        {
        }

        public void Draw(IntPtr renderer, Color rendererColor)
        {
            if (c != null) SDL.SDL_SetRenderDrawColor(renderer, c.R, c.G, c.B, c.A);
            SDL.SDL_RenderDrawPoint(renderer, x, y);
            SDL.SDL_RenderDrawPoints(renderer, trail, trailLen);
            SDL.SDL_SetRenderDrawColor(renderer, rendererColor.R, rendererColor.G, rendererColor.B, rendererColor.A);
        }

        public void Update()
        {
            trailPos++;
            if (trailPos == trailLen) trailPos = 0;
            trail[trailPos] = new SDL.SDL_Point() { x=x, y=y };

            var calcX = new Func<float, float, float>((x, y) => (x + 1e-1f));
            var calcY = new Func<float, float, float>((x, y) => (float)(Math.Pow(x, 2) + (2 * x)));

            x = (int)calcX(x, y);
            y = (int)calcY(x, y);

            age += ageinc;
        }

        public static float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}
