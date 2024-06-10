using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IncidentManagementSystem.Startup))]
namespace IncidentManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
