using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShuttleServiceManagementSystem.Startup))]
namespace ShuttleServiceManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
