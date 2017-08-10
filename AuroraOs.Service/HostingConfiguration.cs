using AuroraOs.Web.Api.Core;
using Microsoft.Owin.Hosting;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace AuroraOs.Service
{
    public class HostingConfiguration : ServiceControl
    {
        private IDisposable _webApplication;

        public bool Start(HostControl hostControl)
        {
            var address = "http://127.0.0.1:9001";
            Trace.WriteLine("Starting the service");
            _webApplication = WebApp.Start<Startup>(address);
            LogManager.GetCurrentClassLogger().Info($"Listening {address}");
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            try
            {
                _webApplication.Dispose();
            }
            catch
            {

            }

            return true;
        }
    }
}
