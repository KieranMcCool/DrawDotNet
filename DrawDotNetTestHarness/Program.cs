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
            window.Show();
        }
    }
}
