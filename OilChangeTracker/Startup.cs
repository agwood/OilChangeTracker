using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OilChangeTracker.Startup))]
namespace OilChangeTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
