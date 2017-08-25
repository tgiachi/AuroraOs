using AuroraOs.Common.Core.Attributes;
using AuroraOs.Engine.Core.Interfaces;
using Jint.Native.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Engine.Core.Services
{
    [AuroraService("System")]
    public class JsService : IJsService
    {
        private Jint.Engine _engine;

        private ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, object> _functions = new Dictionary<string, object>();

        public JsService()
        {
            _engine = new Jint.Engine(cfg => cfg.AllowClr());
        }

        public Task Init()
        {
            return Task.CompletedTask;
        }

        public void RegisterFunction(string name, object del)
        {
            _engine.SetValue(name, del);
            _functions.Add(name, del);
        }

        public object Eval(string value, Dictionary<string, object> vars)
        {
            if (vars != null)
            {
                foreach (var keyValuePair in vars)
                {
                    _engine.SetValue(keyValuePair.Key, keyValuePair.Value);
                }
            }

            try
            {
                return _engine.Execute(value).GetCompletionValue().ToObject();
            }
            catch (Exception e)
            {
                _logger.Error($"Error during executing script {value} => {e.Message}");
                return null;
            }


        }

        public object ParseJsonForVariabile(string json)
        {
            return new JsonParser(_engine).Parse(json);
        }

        public void Dispose()
        {

        }
    }
}
