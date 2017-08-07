using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Engine.Core.Services
{

    [AuroraService(StartAtStartup = true)]
    public class TestService : IAuroraService
    {
        
        public Task<bool> Start()
        {
            Console.WriteLine("OK!");

            return Task.FromResult(true);
        }

        public Task<bool> Stop()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
          
          
        }
    }
}
