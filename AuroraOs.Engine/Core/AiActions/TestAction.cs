using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuroraOs.Common.Core.Attributes;

namespace AuroraOs.Engine.Core.AiActions
{
    [AiAction("test")]
    public class TestAction
    {
        [AiExecutingAction("sayHello")]
        public string SayHello(string name)
        {
            return $"Hi, {name}";
        }
    }
}
