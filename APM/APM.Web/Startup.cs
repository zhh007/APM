using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(APM.Web.Startup))]
namespace APM.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
