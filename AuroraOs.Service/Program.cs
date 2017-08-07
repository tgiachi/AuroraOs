using AuroraOs.WebApi.Core;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace AuroraOs.Service
{
    class Program
    {
        public static int Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
            {
                x.Service<HostingConfiguration>();
                x.RunAsLocalSystem();
                x.SetDescription("Owin + Webapi as Windows service");
                x.SetDisplayName("owin.webapi.test");
                x.SetServiceName("owin.webapi.test");
            });

            return (int)exitCode;
        }
    }
}
