using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Survival.GameEngine
{
    internal class AsyncTimer
    {
        public delegate void Tick();
        public event Tick OnTick;

        public TimeSpan Interval { get; set; }

        private Thread thread;
        private DateTime lastTick;

        public AsyncTimer()
        {
            this.thread = new Thread(this.InternalTask);
        }

        public void Start()
        {
            this.thread.Start();
        }

        public void Stop()
        {
            this.thread.Abort();
        }

        private async void InternalTask()
        {
           while (true)
            {
                this.lastTick = DateTime.Now;
                Application.Current.Dispatcher.Invoke(() => this.OnTick());

                TimeSpan deltaTime = DateTime.Now - this.lastTick;
                await Task.Delay(TimeSpan.FromMilliseconds(this.Interval.TotalMilliseconds / deltaTime.TotalMilliseconds));
            }
        }
    }
}
