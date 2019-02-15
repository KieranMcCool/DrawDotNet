using System;
namespace DrawDotNet.Interfaces
{
    public interface IDrawable: IDisposable
    {
        void Draw();
        void Update();
    }
}
