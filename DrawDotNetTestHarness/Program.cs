using System;
using System.Drawing;
using System.Threading.Tasks;
using DrawDotNet;
using DrawDotNet.Interfaces;
using DrawDotNet.Drawables;
using DrawDotNet.Utilities;
using System.Collections.Generic;

namespace DrawDotNetTestHarness
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var r = Constants.RandomNumberGenerator;
            int size = 800;
            var window = new Window("Test Window", size, size, Color.DodgerBlue);

            var entities = new List<IDrawable>();

            for(int i = 0; i < 10000; i++)
            {
                var rect = new DrawDotNet.Drawables.Rectangle(r.Next(0, size), 
                    r.Next(0, size), r.Next(0, size/4), r.Next(0, size/4), 
                    r.Next(0, 10) > 5, Color.FromArgb(255, r.Next(0,255), 
                    r.Next(0, 255), r.Next(0, 255)));
                entities.Add(rect);
                window.addEntity(rect);
            }

            window.Show();

            Console.Read();
        }
    }
}
