using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Data.IoT.Base
{
    public class BaseMqtt : BaseIot
    {
        public string StateTopic { get; set; }
    }
}
