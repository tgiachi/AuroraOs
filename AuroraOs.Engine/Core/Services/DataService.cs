using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Services.Interfaces;
using AuroraOs.Common.Core.Utils;
using AuroraOs.Engine.Core.Interfaces;
using Chroniton;
using Chroniton.Jobs;
using Chroniton.Schedules;
using Microsoft.Practices.Unity;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Engine.Core.Services
{

    [AuroraService]
    public class DataService : IDataService
    {
        private readonly IEventQueueService _eventQueueService;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly Singularity _singularity;
        private readonly IUnityContainer _container;
 
        private List<IDataGenerator> _generators = new List<IDataGenerator>();

        public DataService(IEventQueueService eventQueueService, IUnityContainer container)
        {
            _eventQueueService = eventQueueService;
            _container = container;

            _singularity = Singularity.Instance;
            _singularity.Start();

            AddDataGenerator();
        }

        public void AddDataGenerator()
        {

            var types = AssemblyUtils.ScanAllAssembliesFromAttribute(typeof(DataGeneratorInfoAttribute));

            foreach (var type in types)
            {
                try
                {
                    var attr = type.GetCustomAttribute<DataGeneratorInfoAttribute>();

                    _logger.Info($"Adding {type.Name} to generators every {attr.Seconds}");

                    _container.RegisterType(type, new ContainerControlledLifetimeManager());


                    var xTime = new EveryXTimeSchedule(TimeSpan.FromSeconds(attr.Seconds));

                    var job = new SimpleParameterizedJob<string>((parameter, scheduledTime) =>
                    {
                        _logger.Info($"Starting dataGenerator {type.Name}");
                        var dataGenerator = _container.RegisterType(type) as IDataGenerator;
                        dataGenerator.Execute();
                    });

                    _singularity.ScheduleParameterizedJob(xTime, job, "", DateTime.Now.Add(TimeSpan.FromSeconds(attr.Seconds)));
                    var gen = _container.Resolve(type) as IDataGenerator;
                    gen.Execute();
                    _generators.Add(gen);

                }
                catch (Exception ex)
                {
                    _logger.Error($"Error during add dataGenerator => {ex.Message}");
                }
            }

        }

        public void Dispose()
        {

        }

        public List<string> GetAvailableGenerators()
        {
            var list = new List<string>();

            _generators.ForEach(x => list.Add(x.GetType().Name));

            return list;
        }

        public void StartGenerator(string name)
        {
            var dg = _generators.FirstOrDefault(s => s.GetType().Name == name);

            try
            {
                dg.Execute();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error during excuting ${name} => {ex.Message}");
            }
        }
    }
}
