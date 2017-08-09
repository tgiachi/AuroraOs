using AuroraOs.Common.Core.Data.IoT.Scene;
using AuroraOs.Engine.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AuroraOs.Web.Api.Core.Controllers
{
    public class ScenesController : ApiController
    {
        private readonly ISceneService _sceneService;


        public ScenesController(ISceneService sceneService)
        {
            _sceneService = sceneService;

        }

        [HttpGet]
        public List<SceneData> All()
        {
            return _sceneService.Scenes();
        }


        [HttpPost]
        public string StartScene(string name)
        {
            _sceneService.StartScene(name);

            return "ok";
        }
    }
}
