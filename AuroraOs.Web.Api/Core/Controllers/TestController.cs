using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AuroraOs.Web.Api.Core.Controllers
{
    public class TestController : ApiController
    {

        [HttpGet]

        public string Test()
        {
            return "Ok";
        }
    }
}
