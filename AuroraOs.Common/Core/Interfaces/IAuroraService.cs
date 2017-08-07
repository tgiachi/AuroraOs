using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Interfaces
{
    public interface IAuroraService : IDisposable
    {
        Task<bool> Start();

        Task<bool> Stop();
    }
}
