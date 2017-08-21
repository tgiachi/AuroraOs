using System;
using System.IO;
using NLog;
using AuroraOs.Common.Core.Data.Config;
using AuroraOs.Common.Core.Utils;
using System.Collections.Generic;

namespace AuroraOs.Common.Core.Manager
{
    public class ConfigManager
    {
		private static readonly Lazy<ConfigManager> lazy = new Lazy<ConfigManager>(() => new ConfigManager());
		public static ConfigManager Instance { get { return lazy.Value; } }

        private AuroraConfig _config;
		
        private string _homeDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}{Path.DirectorySeparatorChar}AuroraOs";

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ConfigManager()
        {
            Init();

            Load();
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

            GetPath("Config");



                
        }

        private void Load()
        {
            var defaultConfig = GetPath("Config") + "auroraos.config";
            if (!File.Exists(defaultConfig))
            {
                _config = new AuroraConfig();
                File.WriteAllText(defaultConfig, _config.ToJson());
            }

            _config = File.ReadAllText(defaultConfig).FromJson<AuroraConfig>();

        }


        private void Save()
        {
            var defaultConfig = GetPath("Config") + "auroraos.config";
            File.WriteAllText(defaultConfig, _config.ToJson());
        }


        public Dictionary<string, string> GetConfig<T>()
        {
            if (_config.Configs.ContainsKey(typeof(T).Name))
            {
                return _config.Configs[typeof(T).Name];
            }

            return null;
        }

        public string GetConfigValue<T>(string key, string defaultValue = "")
        {
            if (_config.Configs.ContainsKey(typeof(T).Name))
            {
                var cfg = _config.Configs[typeof(T).Name];

                if (cfg.ContainsKey(key))
                    return cfg[key];
                else
                {
                    SetConfig<T>(key, defaultValue);
                    return defaultValue;
                }
                  
            }
            else
            {
                if (!string.IsNullOrEmpty(defaultValue))
                {
                    SetConfig<T>(key, defaultValue);
                    return defaultValue;
                }
            }

            return null;
        }

        public void SetConfig<T>(string key, string value)
        {
            if (!_config.Configs.ContainsKey(typeof(T).Name))
                _config.Configs.Add(typeof(T).Name, new Dictionary<string, string>());

            _config.Configs[typeof(T).Name].Add(key, value);

            Save();
        }

        public string GetPath(string path)
        {
            var fullPath = _homeDirectory + Path.DirectorySeparatorChar + path + Path.DirectorySeparatorChar;
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            return fullPath;
        }
    }
}
