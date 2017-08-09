using AuroraOs.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AuroraOs.Web.Api.Core.Controllers
{
    public class DiagnosticController : ApiController
    {
        [HttpGet]
        public string Version()
        {
            return AppUtils.AppVersion;
        }

        [HttpGet]
        public string ApplicationName()
        {
            return AppUtils.AppName;
        }
    }
}
