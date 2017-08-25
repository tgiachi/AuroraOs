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
    public class AssistantController : ApiController
    {
        private IAiService _aiService;

        public AssistantController(IAiService aiService)
        {
            _aiService = aiService;
        }
        public HttpResponseMessage Speak(string text)
        {
            _aiService.Speak(text);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
