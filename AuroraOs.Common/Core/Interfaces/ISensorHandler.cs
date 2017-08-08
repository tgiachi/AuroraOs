using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Interfaces
{
    public interface ISensorHandler
    {
        Task<bool> AddSensor(object obj);
    }
}
