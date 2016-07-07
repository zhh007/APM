using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(APM.Admin.Startup))]
namespace APM.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
