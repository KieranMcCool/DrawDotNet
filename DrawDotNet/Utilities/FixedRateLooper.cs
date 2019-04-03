using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DrawDotNet.Utilities
{
    public class FixedRateLooper
    {
        string name;
        Stopwatch timer;
        Action action;
        bool printLog;

        public long CyclesPerSecond { get; private set; }

        Task task;
        CancellationTokenSource cancellationTokenSource;
        CancellationToken cancellationToken;

        public FixedRateLooper(string name, long cyclesPerSecond, Action action, bool printLog): this(name, cyclesPerSecond, action)
        {
            this.printLog = false;
        }

        public FixedRateLooper(string name, long cyclesPerSecond, Action action): this(cyclesPerSecond, action)
        {
            this.name = name;
        }

        public FixedRateLooper(long cyclesPerSecond, Action action)
        {
            printLog = true;
            timer = new Stopwatch();
            this.CyclesPerSecond = cyclesPerSecond;
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            this.action = action;
            task = new Task(TaskLoop, cancellationToken);
            timer.Start();
        }

        public void Join() 
        {
            try
            {
                task.Wait();
            } catch (Exception) { }
        }

        public void Cancel()
        {
            timer.Stop();
            cancellationTokenSource.Cancel();
        }

        public void Start()
        {
            task.Start();
        }

        public void StartSynchronous()
        {
            TaskLoop();
        }

        private void TaskLoop()
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var frameStart = timer.ElapsedMilliseconds;
                action();
                var frameEnd = timer.ElapsedMilliseconds;

                var frameTime = frameEnd - frameStart;

                if (printLog) Console.WriteLine("[{0}] - {1}ms", name, frameTime);
                var timeLeft = 1000 / CyclesPerSecond - frameTime;

                if (CyclesPerSecond != -1)
                {
                    if (timeLeft > 0) Thread.Sleep((int)timeLeft);
                }
            }
        }

    }
}
