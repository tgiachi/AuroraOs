using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ApiAiSDK;
using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Data.Events;
using AuroraOs.Common.Core.Manager;
using AuroraOs.Common.Core.Services.Interfaces;
using AuroraOs.Common.Core.Utils;
using AuroraOs.Engine.Core.Interfaces;
using AuroraOs.Entities.Core.Repositories.Interfaces;
using Foundatio.Utility;
using Microsoft.Practices.Unity;
using NLog;
using Org.BouncyCastle.Asn1.Crmf;

namespace AuroraOs.Engine.Core.Services
{
    [AuroraService("Ai")]
    public class AiService : IAiService
    {

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public readonly Dictionary<string, Type> _actions = new Dictionary<string, Type>();

        [Dependency]
        public IEventQueueService EventQueue { get; set; }

        [Dependency]
        public IUnityContainer UnityContainer { get; set; }

        public IConfigValuesRepository ConfigValuesRepository { get; set; }



        private ApiAi _apiAiClient;
        private string _accessToken;


        public Task Init()
        {
            ScanActions();

            _accessToken = ConfigManager.Instance.GetConfigValue<AiService>("accessToken", "33a00f8b46464c158a41278dfd37ece8");

            var config = new AIConfiguration(_accessToken, SupportedLanguage.Italian);

            _apiAiClient = new ApiAi(config);

            EventQueue.Subscribe<AiRequestSpeechEvent>(e =>
            {
                Speak(e.Text);
            });

            return Task.CompletedTask;
        }

        private void ScanActions()
        {
            var types = AssemblyUtils.ScanAllAssembliesFromAttribute(typeof(AiActionAttribute));

            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<AiActionAttribute>();

                foreach (var methodInfo in type.GetMethods())
                {
                    var aiExecutingAcAttr = methodInfo.GetCustomAttribute<AiExecutingActionAttribute>();

                    if (aiExecutingAcAttr != null)
                    {
                        _logger.Debug($"Ai Action found: {attr.Name}.{aiExecutingAcAttr.Name}");

                        try
                        {
                            if (!UnityContainer.IsRegistered(type))
                                UnityContainer.RegisterType(type, new PerResolveLifetimeManager());

                            _actions.Add($"{attr.Name}.{aiExecutingAcAttr.Name}", type);

                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Error during registering {attr.Name}.{aiExecutingAcAttr.Name} => {ex}");

                        }
                    }

                }
            }

        }

        public void Speak(string text)
        {
            var result = _apiAiClient.TextRequest(text);

            if (!result.IsError)
            {
                if (!string.IsNullOrEmpty(result.Result.Action))
                {
                    if (_actions.ContainsKey(result.Result.Action))
                    {
                        var action = _actions[result.Result.Action];

                        //TODO
                    }
                    
                }

                EventQueue.Publish(new AiSpeechResultEvent()
                {
                    ResultText = result.Result.Fulfillment.DisplayText,
                    ActionName = result.Result.Action

                });
            }


        }

        public void Dispose()
        {

        }
    }
}
