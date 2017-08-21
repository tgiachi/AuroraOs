using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuroraOs.Common.Core.Interfaces;

namespace AuroraOs.Engine.Core.Interfaces
{
    public interface ISonyBraviaService : IAuroraService
    {
        Task<bool> SetPinCode(string pinCode);



        Task<bool> RequestPinCode();
    }
}
