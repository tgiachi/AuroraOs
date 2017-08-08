using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Interfaces;
using AuroraOs.Common.Core.Utils;
using Microsoft.Practices.Unity;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Manager
{
    public class AuroraManager
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();
        private List<Type> _delayedServices = new List<Type>();


        public IUnityContainer Container { get; set; }

        public AuroraManager()
        {
            Init();
        }

        private void Init()
        {
            var cfg = ConfigManager.Instance;


            Container = new UnityContainer();
            _logger.Info("Scanning assemblies for services");


            ScanServices();

            StartDelayedServices();
        }

        private void ScanServices()
        {

            var services = AssemblyUtils.ScanAllAssembliesFromAttribute(typeof(AuroraServiceAttribute));

            _logger.Info($"Found {services.Count} services");

            services.ForEach(InitService);

        }

        private void StartDelayedServices()
        {
            _delayedServices.ForEach(async service =>
            {
                try
                {
                    var srv = Container.Resolve(service) as IDelayedService;

                    if (srv != null)
                        await srv?.Start();
                    else
                        _logger.Warn($"Service {service.Name} not implement IDelayedService interface");
                }
                catch (Exception ex)
                {
                    _logger.Error($"Error during post-start service {service.Name} => {ex}");
                    _logger.Error(ex);
                }


            });
        }

        private void InitService(Type type)
        {
            try
            {
                _logger.Info($"Initializing service {type.Name}");
                var serviceAttribute = type.GetCustomAttribute<AuroraServiceAttribute>();

                LifetimeManager ltm;

                if (serviceAttribute.ServiceType == Enums.AuroraServiceType.Singleton)
                    ltm = new ContainerControlledLifetimeManager();
                else
                    ltm = new PerResolveLifetimeManager();

                _logger.Debug($"Registering {type.Name} as {serviceAttribute.ServiceType}");


                if (type.GetInterfaces().Count() > 0)
                {
                    Container.RegisterType(type.GetInterfaces()[0], type, ltm);
                }
                else
                {
                    Container.RegisterType(type, ltm);
                }

                if (serviceAttribute.ServiceType == Enums.AuroraServiceType.Singleton)
                    Container.Resolve(type);

                if (serviceAttribute.StartAtStartup && serviceAttribute.ServiceType == Enums.AuroraServiceType.Singleton)
                {
                    _logger.Debug($"Adding service {type.Name} to delayed service");
                    _delayedServices.Add(type);
                }

            }
            catch (Exception ex)
            {
                _logger.Fatal($"Error during initializing service {type.Name} => {ex}");
                _logger.Fatal(ex);
            }
        }

    }
}
