using AuroraOs.Engine.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AuroraOs.Web.Api.Core.Controllers
{
    public class MediaServiceController : ApiController
    {
        private IMediaService _mediaService;

        public MediaServiceController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        public bool ScanDirectory(string path)
        {
            _mediaService.ScanDirectory(path);

            return true;
        }
    }
}
