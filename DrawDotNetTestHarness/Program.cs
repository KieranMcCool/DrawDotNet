using System;
using DrawDotNet;

namespace DrawDotNetTestHarness
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var window = new Window(400, 400);
            window.Show();
            Console.Read();
        }
    }
}
