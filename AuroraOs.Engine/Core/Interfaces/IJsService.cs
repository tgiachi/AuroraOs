using AuroraOs.Common.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Engine.Core.Interfaces
{
    public interface IJsService : IAuroraService
    {
        void RegisterFunction(string name, object del);

        object Eval(string value, Dictionary<string, object> vars);

        object ParseJsonForVariabile(string json);
    }
}
