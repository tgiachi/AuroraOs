using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AiExecutingActionAttribute : Attribute
    {
        public string Name { get; set; }

        public AiExecutingActionAttribute(string name)
        {
            Name = name;
        }
    }
}
