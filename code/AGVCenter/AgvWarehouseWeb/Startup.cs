using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AgvWarehouseWeb.Startup))]
namespace AgvWarehouseWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
