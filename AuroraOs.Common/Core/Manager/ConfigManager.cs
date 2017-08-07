using System;
using System.IO;
using NLog;

namespace AuroraOs.Common.Core.Manager
{
    public class ConfigManager
    {
		private static readonly Lazy<ConfigManager> lazy = new Lazy<ConfigManager>(() => new ConfigManager());
		public static ConfigManager Instance { get { return lazy.Value; } }


		
        private string _homeDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}{Path.DirectorySeparatorChar}AuroraOs";

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ConfigManager()
        {
            Init();   
        }

        private void Init()
        {
            _logger.Info("Checking home directory");

            var path =  Environment.GetEnvironmentVariable("AURORAOS_HOME");

            if (!string.IsNullOrEmpty(path))
            {
                _homeDirectory = path + Path.DirectorySeparatorChar + "AuroraOs";
            }

            _logger.Info($"Home directory is {_homeDirectory}");

            if (!Directory.Exists(_homeDirectory))
                Directory.CreateDirectory(_homeDirectory);


                
        }

        public string GetPath(string path)
        {
            var fullPath = _homeDirectory + Path.DirectorySeparatorChar + path;
            if (Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            return fullPath;
        }
    }
}
