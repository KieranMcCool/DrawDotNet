using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DrawDotNet.Utilities
{
    public class FixedRateLooper
    {

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

        public FixedRateLooper(long cyclesPerSecond, Action action)
        {
            timer = new Stopwatch();
            this.cyclesPerSecond = cyclesPerSecond;
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            this.action = action;
            task = new Task(taskLoop, cancellationToken);
        }



        public void Join() 
        {
            task.Wait();
        }

        public void Cancel()
        {
            cancellationTokenSource.Cancel();
        }

        public void Start()
        {
            task.Start();
        }

        private void taskLoop()
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                timer.Reset();
                action();
                if (cyclesPerSecond != -1) 
                    Thread.Sleep((int)((1000 / cyclesPerSecond) - timer.ElapsedMilliseconds));
            }
        }

    }
}
