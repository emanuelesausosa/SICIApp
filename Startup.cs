using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SICIApp.Startup))]
namespace SICIApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
