using AuroraOs.Common.Core.Data.Dto;
using AuroraOs.Common.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Entities.Core.Repositories.Interfaces
{
    public interface IConfigValuesRepository : IAuroraService
    {
        bool AddConfigValue(string name, object value);

        List<ConfigDto> All();

        ConfigDto GetConfigByName(string name);

    }
}
