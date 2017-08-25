using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using AuroraOs.Common.Core.Data.Dto;
using AuroraOs.Common.Core.Manager;

namespace AuroraOs.Web.Api.Core.Controllers
{
    public class ServicesController : ApiController
    {
        private readonly AuroraManager _auroraManager;
        public ServicesController(AuroraManager auroraManager)
        {
            _auroraManager = auroraManager;
        }

        public List<ServiceInfo> GetServices()
        {
            return (from auroraManagerService in _auroraManager.Services
                    from type in auroraManagerService.Value
                    select new ServiceInfo()
                    {
                        Name = type.Name,
                        Category = auroraManagerService.Key
                    }).ToList();
        }
    }
}
