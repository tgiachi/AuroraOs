using AuroraOs.Common.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AuroraServiceAttribute : Attribute
    {
        public AuroraServiceType ServiceType { get; set; }

        public bool StartAtStartup { get; set; }
        
        public string Category { get; set; }

        public AuroraServiceAttribute(string category,AuroraServiceType serviceType = AuroraServiceType.Singleton, bool startAtStartup = false)
        {
            Category = category;
            ServiceType = serviceType;
            StartAtStartup = startAtStartup;
        }
    }
}
