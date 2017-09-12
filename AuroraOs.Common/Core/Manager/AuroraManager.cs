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
using Microsoft.Practices.ObjectBuilder2;

namespace AuroraOs.Common.Core.Manager
{
    public class AuroraManager
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly List<Type> _delayedServices = new List<Type>();
        private readonly List<Type> _singletonServices = new List<Type>();

        public Dictionary<string, List<Type>> Services { get; set; }

        public IUnityContainer Container { get; set; }

        public AuroraManager()
        {
            Services = new Dictionary<string, List<Type>>();

            Init();
        }



        private void Init()
        {
            var cfg = ConfigManager.Instance;


            Container = new UnityContainer();
            Container.RegisterInstance<IUnityContainer>(Container);
            Container.RegisterInstance(this);

            _logger.Info("Scanning assemblies for services");

            ScanRepositories();

            ScanServices();

            StartDelayedServices();
        }

        private void ScanRepositories()
        {
            var repos = AssemblyUtils.ScanAllAssembliesFromAttribute(typeof(AuroraReporitoryAttribute));


            foreach (var type in repos)
            {
                if (type.GetInterfaces().Any())
                {
                    var inter = type.GetInterfaces().FirstOrDefault(s => s.Name.Contains(type.Name));

                    Container.RegisterType(inter, type, new PerResolveLifetimeManager());
                }
                else
                {
                    Container.RegisterType(type, new PerResolveLifetimeManager());
                }

                _logger.Info($"Registered repository {type.Name}");
            }
        }

        private void ScanServices()
        {

            var services = AssemblyUtils.ScanAllAssembliesFromAttribute(typeof(AuroraServiceAttribute));

            _logger.Info($"Found {services.Count} services");

            services.ForEach(InitService);

        }

        private void StartDelayedServices()
        {
            _singletonServices.ForEach(s =>
            {

                try
                {
                    Task.Run(() => Container.Resolve(s));
                  ;
                }
                catch (Exception ex)
                {
                    _logger.Error($"Error during resolve service {s.Name} => {ex}");
                }

            });

            _singletonServices.ForEach(async service =>
            {
                try
                {
                    var ausService = Container.Resolve(service) as IAuroraService;

                    if (ausService != null)
                        Task.Run(() => {
                            try
                            {
                                ausService?.Init();
                            }
                            catch(Exception ex)
                            {
                                _logger.Error($"Error during post-start service {service.Name} => {ex}");
                                _logger.Error(ex);
                            }
                           
                        }
                        );

                    var srv = Container.Resolve(service) as IDelayedService;

                    if (srv != null)
                        await srv?.Start();
                    //else
                    //    _logger.Warn($"Service {service.Name} not implement IDelayedService interface");



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
                    ltm = new ContainerControlledLifetimeManager();

                _logger.Debug($"Registering {type.Name} as {serviceAttribute.ServiceType}");


                if (type.GetInterfaces().Any())
                {
                    var inter = type.GetInterfaces().FirstOrDefault(s => s.Name.Contains(type.Name));

                    Container.RegisterType(inter, type, ltm);
                }
                else
                {
                    Container.RegisterType(type, ltm);
                }

                if (serviceAttribute.ServiceType == Enums.AuroraServiceType.Singleton)
                    _singletonServices.Add(type);

                if (!Services.ContainsKey(serviceAttribute.Category))
                    Services.Add(serviceAttribute.Category, new List<Type>());

                Services[serviceAttribute.Category].Add(type);

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
