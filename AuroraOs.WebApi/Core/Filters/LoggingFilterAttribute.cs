using AuroraOs.WebApi.Core.Extensions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace AuroraOs.WebApi.Core.Filters
{
    public class LoggingFilterAttribute : ActionFilterAttribute
    {

        private Logger _logger = LogManager.GetCurrentClassLogger();
        
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            _logger.Info($"{actionExecutedContext.Request.GetClientIpString()} - [{actionExecutedContext.Request.Method}] - {actionExecutedContext.Request.RequestUri} - ");

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
