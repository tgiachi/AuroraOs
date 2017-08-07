using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Attributes
{
    /// <summary>
    /// Attribute for write 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MongoIndexAttribute : Attribute
    {
    }
}
