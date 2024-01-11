using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Survival.GameEngine
{
    internal class AsyncTimer
    {
        public delegate void Tick();
        public event Tick OnTick;

        public TimeSpan Interval { get; set; }

        private Task task;
        private DateTime lastTick;

        public void Start()
        {
            this.task = this.InternalTask();
           // this.task.Start();
        }

        public void Stop()
        {
            this.task.Dispose();
            this.task = null;
        }

        private async Task InternalTask()
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
