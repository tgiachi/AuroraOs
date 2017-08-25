using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AiActionAttribute : Attribute
    {
        public string Name { get; set; }

        public AiActionAttribute(string name)
        {
            Name = name;
        }
    }
}
