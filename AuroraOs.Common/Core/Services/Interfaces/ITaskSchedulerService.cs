using AuroraOs.Common.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Services.Interfaces
{
    public interface ITaskSchedulerService : IAuroraService
    {
        void QueueTask(Task task, ThreadPriority priority);

    }
}
