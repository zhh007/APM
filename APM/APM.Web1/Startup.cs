using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(APM.Web1.Startup))]
namespace APM.Web1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
