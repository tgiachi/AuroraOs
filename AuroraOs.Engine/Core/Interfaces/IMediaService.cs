using AuroraOs.Common.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Engine.Core.Interfaces
{
    public interface IMediaService : IAuroraService
    {
        void ScanDirectory(string directory);
    }
}
