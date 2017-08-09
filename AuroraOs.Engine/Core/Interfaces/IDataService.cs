using AuroraOs.Common.Core.Interfaces;
using AuroraOs.Common.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Engine.Core.Interfaces
{
    public interface IDataService : IAuroraService
    {
        void AddDataGenerator();

        void StartGenerator(string name);

        List<string> GetAvailableGenerators();
    }
}
