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
        private readonly IWithingService _withingService;

        public OAuthController(INestService nestService, IWithingService withingService)
        {
            _nestService = nestService;
            _withingService = withingService;
        }

        [HttpGet]
        public HttpResponseMessage Nest(string code, string state)
        {
            _nestService.ReceiveCode(code);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        public HttpResponseMessage withings(string userId, string oauth_Token, string Oauth_verifier)
        {
            _withingService.InitializeAuth(userId, oauth_Token, Oauth_verifier);
            return Request.CreateResponse(HttpStatusCode.OK, "OK");
        }
    }
}
