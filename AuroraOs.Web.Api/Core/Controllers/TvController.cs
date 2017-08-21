using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using AuroraOs.Engine.Core.Interfaces;

namespace AuroraOs.Web.Api.Core.Controllers
{
    public class TvController : ApiController
    {
        private ISonyBraviaService _sonyBraviaService;
        public TvController(ISonyBraviaService sonyBraviaService)
        {
            _sonyBraviaService = sonyBraviaService;
        }

        public async Task<bool> RequestPinToTv()
        {
            return await _sonyBraviaService.RequestPinCode();
        }


        public async Task<bool> SetPinCode(string pinCode)
        {
            return await _sonyBraviaService.SetPinCode(pinCode);
        }
    }

}
