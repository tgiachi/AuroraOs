
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Data.IoT.Base
{
    public class BaseSensor : BaseIot
    {
        public string UnitOfMeasurement { get; set; }

        public long PollingTime { get; set; } = 1000;


    }
}
