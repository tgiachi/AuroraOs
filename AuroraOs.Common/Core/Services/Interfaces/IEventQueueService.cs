using AuroraOs.Common.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Services.Interfaces
{

    /// <summary>
    /// Event bus
    /// </summary>
    public interface IEventQueueService : IAuroraService
    {
        bool Subscribe<T>(Action<T> action) where T : class;

        Task Publish<T>(T obj) where T : class;

    }
}
