using System;
using System.Drawing;
using SDL2;

namespace DrawDotNet.Utilities
{
    public static class ColorHelpers
    {
        public static SDL.SDL_Color netColourToSDL(Color c)
        {
            return new SDL.SDL_Color() { r = c.R, g = c.G, b = c.B, a = c.A };
        }

        public static uint netColourToUInt(Color c)
        {
            return (uint)((c.R << 2) + (c.G << 2) + (c.B << 2) + (c.A));
        }

        public static void setRenderColour(IntPtr r, Color c)
        {
            SDL.SDL_SetRenderDrawColor(r, c.R, c.G, c.B, c.A);
        }
    }
}
