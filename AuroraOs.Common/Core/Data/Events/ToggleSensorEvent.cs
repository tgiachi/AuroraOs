using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Data.Events
{
    public class ToggleSensorEvent
    {
        public string Name { get; set; }

        public string State { get; set; }
    }
}
