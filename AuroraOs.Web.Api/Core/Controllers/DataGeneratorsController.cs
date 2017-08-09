using AuroraOs.Engine.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AuroraOs.Web.Api.Core.Controllers
{
    public class DataGeneratorsController : ApiController
    {
        private IDataService _dataService;

        public DataGeneratorsController(IDataService dataService)
        {
            _dataService = dataService;
        }



        [HttpGet]
        public List<string> AvailableGenerators()
        {
            return _dataService.GetAvailableGenerators();

        }


        [HttpPost]
        public string StartGenerator(string name)
        {
            _dataService.StartGenerator(name);
            return "ok";
        }


    }
}
