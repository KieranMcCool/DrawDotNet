using System;
using System.Drawing;

namespace DrawDotNet.Interfaces
{
    public interface IDrawable: IDisposable
    {
        void Draw(IntPtr renderer, Color rendererColor);
        void Update();
    }
}
