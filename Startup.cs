using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Owin;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Web.Hosting;

[assembly: OwinStartup(typeof(Doanphanmem.Startup))]

namespace Doanphanmem
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();


        }
        
    }
}
