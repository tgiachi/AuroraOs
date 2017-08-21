using AuroraOs.Entities.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuroraOs.Common.Core.Data.Dto;
using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Enums;
using AuroraOs.Common.Core.Services.Interfaces;
using AuroraOs.Entities.Core.Entities;
using AuroraOs.Common.Core.Utils;

namespace AuroraOs.Entities.Core.Repositories
{

    [AuroraService(AuroraServiceType.PerRequest)]
    public class ConfigValuesRepository : IConfigValuesRepository
    {

        private readonly INoSqlService _dbContext;

        public ConfigValuesRepository(INoSqlService noSqlServie)
        {
            _dbContext = noSqlServie;
        }

       

        public List<ConfigDto> All()
        {
            var list = _dbContext.SelectAll<ConfigValue>().ToList();

            var res = new List<ConfigDto>();

            list.ForEach(value => res.Add(new ConfigDto()
            {
                Name = value.Name,
                Type = value.Type,
                Value = value.Value
            }));

            return res;
        }

        public void Dispose()
        {
          
        }

        public bool AddConfigValue(string name, object value)
        {
            var obj = GetConfigValueByName(name);
            var serialz = value.ToJson();

            if (obj == null)
            {

                obj = new ConfigValue()
                {
                    Name = name,
                    Type = value.GetType().FullName,
                    Value = serialz

                };

                _dbContext.Insert(obj);

                return true;
            }
            else
            {
                obj.Value = serialz;
                obj.Type = value.GetType().FullName;

                _dbContext.Update(obj);

                return true;
            }
        }

        private ConfigValue GetConfigValueByName(string name)
        {
            return _dbContext.Select<ConfigValue>(value => value.Name == name).FirstOrDefault();
        }

        public ConfigDto GetConfigByName(string name)
        {
            var obj = GetConfigValueByName(name);

            if (obj != null)
            {
                return new ConfigDto()
                {
                    Name = obj.Name,
                    Type = obj.Type,
                    Value = obj.Value
                };
            }

            return null;
        }
    }
}
