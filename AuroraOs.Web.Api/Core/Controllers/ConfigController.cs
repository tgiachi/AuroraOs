using AuroraOs.Common.Core.Data.Dto;
using AuroraOs.Entities.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AuroraOs.Web.Api.Core.Controllers
{
    class ConfigController : ApiController
    {
        private IConfigValuesRepository _configValuesRepository;

        public ConfigController(IConfigValuesRepository configValuesRepository)
        {
            _configValuesRepository = configValuesRepository;
        }



        [HttpGet]
        public List<ConfigDto> All()
        {
            return _configValuesRepository.All();

        }

    }
}
