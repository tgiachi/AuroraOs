using AuroraOs.Common.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AuroraOs.Common.Core.Attributes;
using NLog;

namespace AuroraOs.Common.Core.Services
{
    [AuroraService]
    public class TaskSchedulerService : ITaskSchedulerService
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();


        public TaskSchedulerService()
        {
            _logger.Info($"Task scheduler max councurrent tasks: {Math.Max(1, Environment.ProcessorCount)}");
        }

        public void Dispose()
        {
            
        }

        public void QueueTask(Task task, ThreadPriority priority = ThreadPriority.Normal)
        {
            switch (priority)
            {
                case ThreadPriority.Lowest:
                    PriorityScheduler.Lowest.EnqueueTask(task);
                    break;
                case ThreadPriority.BelowNormal:
                    PriorityScheduler.BelowNormal.EnqueueTask(task);
                    break;
                case ThreadPriority.Normal:
                    PriorityScheduler.Normal.EnqueueTask(task);
                    break;
                case ThreadPriority.AboveNormal:
                    PriorityScheduler.AboveNormal.EnqueueTask(task);
                    break;
                case ThreadPriority.Highest:
                    PriorityScheduler.Highest.EnqueueTask(task);
                    break;
                default:
                    break;
            }
           
        }
    }
}
