using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using AuroraOs.Engine.Core.Interfaces;

namespace AuroraOs.Web.Api.Core.Controllers
{
    public class OAuthController : ApiController
    {

        private readonly INestService _nestService;
        public OAuthController(INestService nestService)
        {
            _nestService = nestService;
        }

        [HttpGet]
        public HttpResponseMessage Nest(string code, string state)
        {
            _nestService.ReceiveCode(code);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
