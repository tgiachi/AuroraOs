using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Interfaces;
using AuroraOs.Common.Core.Services.Interfaces;
using AuroraOs.Common.Core.Utils;
using AuroraOs.Engine.Core.Interfaces;
using Microsoft.Practices.Unity;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Engine.Core.Services
{
    [AuroraService("Audio")]
    public class MediaService : IMediaService
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();

        private Dictionary<string, List<Type>> _parsers = new Dictionary<string, List<Type>>();
        private readonly ITaskSchedulerService _taskScheduler;
        private readonly IUnityContainer _container;

        public MediaService(IUnityContainer container, ITaskSchedulerService taskScheduler)
        {
            _container = container;
            _taskScheduler = taskScheduler;

        }

        public Task Init()
        {
            var types = AssemblyUtils.ScanAllAssembliesFromAttribute(typeof(MediaServiceTypeAttribute));

            _logger.Info($"Found {types.Count} media types parsers");

            foreach(var type in types)
            {
                try
                {
                    var attr = type.GetCustomAttribute<MediaServiceTypeAttribute>();
                    var fExt = attr.FileExtension.ToLower();

                    _logger.Info($"Parser {type.Name} for {fExt}");

                    if (!_parsers.ContainsKey(fExt))
                        _parsers.Add(fExt, new List<Type>());

                    _parsers[fExt].Add(type);
                    _container.RegisterType(type, new PerResolveLifetimeManager());

                }
                catch(Exception ex)
                {
                    _logger.Error($"Error during init media parser {type.Name}");
                    _logger.Error(ex);
                }
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
           
        }



        public void ScanDirectory(string directory)
        {
            try
            {
                _logger.Info($"Starting scanning to directory {directory}");

                var files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

                foreach(var file in files)
                {
                    var fExt = Path.GetExtension(file).Remove(0,1);

                    if (_parsers.ContainsKey(fExt))
                    {
                        foreach (var parserType in _parsers[fExt])
                        {
                            var parser = _container.Resolve(parserType) as IMediaParser;
                            try
                            {
                                _taskScheduler.QueueTask(parser.Parse(file));
                            }
                            catch(Exception ex)
                            {
                                _logger.Error($"Error during parsing {file} => {ex.Message}");
                            }
                          
                        }
                    }
                    else
                    {
                        _logger.Debug($"Parser for file {file} not found!");
                    }

                }

            }
            catch(Exception ex)
            {
                _logger.Error($"Error during scanning directory {directory}");
                _logger.Error(ex);
            }

        }
    }
}
