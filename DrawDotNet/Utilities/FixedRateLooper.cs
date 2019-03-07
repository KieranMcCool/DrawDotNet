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
        long cyclesPerSecond;

        public long CyclesPerSecond
        {
            get => cyclesPerSecond;
            set => cyclesPerSecond = value;
        }

        Task task;
        CancellationTokenSource cancellationTokenSource;
        CancellationToken cancellationToken;

        public FixedRateLooper(string name, long cyclesPerSecond, Action action): this(cyclesPerSecond, action)
        {
            this.name = name;
        }

        public FixedRateLooper(long cyclesPerSecond, Action action)
        {
            timer = new Stopwatch();
            this.cyclesPerSecond = cyclesPerSecond;
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            this.action = action;
            task = new Task(TaskLoop, cancellationToken);
            timer.Start();
        }

        public void Join() 
        {
            task.Wait();
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

                Console.WriteLine("[{0}] - {1}ms", name, frameTime);
                var timeLeft = 1000 / cyclesPerSecond - frameTime;

                if (cyclesPerSecond != -1)
                {
                    if (timeLeft > 0) Thread.Sleep((int)timeLeft);
                }
            }
        }

    }
}
