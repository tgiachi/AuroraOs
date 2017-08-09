using AuroraOs.Common.Core.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Services
{
    public class PriorityScheduler : TaskScheduler
    {
        public static PriorityScheduler AboveNormal = new PriorityScheduler(ThreadPriority.AboveNormal);
        public static PriorityScheduler BelowNormal = new PriorityScheduler(ThreadPriority.BelowNormal);
        public static PriorityScheduler Lowest = new PriorityScheduler(ThreadPriority.Lowest);
        public static PriorityScheduler Normal = new PriorityScheduler(ThreadPriority.Normal);
        public static PriorityScheduler Highest = new PriorityScheduler(ThreadPriority.Highest);

        private BlockingCollection<Task> _tasks = new BlockingCollection<Task>();
        private Thread[] _threads;
        private ThreadPriority _priority;
        private readonly int _maximumConcurrencyLevel = Math.Max(1, Environment.ProcessorCount);

        public PriorityScheduler(ThreadPriority priority)
        {
            _priority = priority;
        }

        public PriorityScheduler()
        {
            _priority = ThreadPriority.Normal;
        }

        public override int MaximumConcurrencyLevel
        {
            get { return _maximumConcurrencyLevel; }
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return _tasks;
        }


        public void EnqueueTask(Task task)
        {
            QueueTask(task);

        }
        protected override void QueueTask(Task task)
        {
            _tasks.Add(task);

            if (_threads == null)
            {
                _threads = new Thread[_maximumConcurrencyLevel];
                for (int i = 0; i < _threads.Length; i++)
                {
                    int local = i;
                    _threads[i] = new Thread(() =>
                    {
                        foreach (Task t in _tasks.GetConsumingEnumerable())
                            TryExecuteTaskInline(t, true);
                    })
                    {
                        Name = string.Format("PriorityScheduler: ", i),
                        Priority = _priority,
                        IsBackground = true
                    };
                    _threads[i].Start();
                }
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false; // we might not want to execute task that should schedule as high or low priority inline
        }

        public void Dispose()
        {

        }
    }
}
