using AuroraOs.Common.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Entities.Core.Entities
{
    [MongoDocument("configValues")]
    public class ConfigValue
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }
    }
}
