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
    public interface IEventBusService : IAuroraService
    {
        Task SubscribeAsync<T>(Action<T> handler) where T : class;

        Task PublishAsync(Type messageType, object message, TimeSpan? delay = default(TimeSpan?), CancellationToken cancellationToken = default(CancellationToken));

        Task PublishAsync<T>(T message, TimeSpan? delay = default(TimeSpan?)) where T : class;

    }
}
