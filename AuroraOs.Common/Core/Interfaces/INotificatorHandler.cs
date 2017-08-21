using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuroraOs.Common.Core.Data;

namespace AuroraOs.Common.Core.Interfaces
{
    public interface INotificatorHandler : IDisposable
    {
        Task<bool> Notificate(NotificationData data);

        Task<bool> Init();
    }
}
