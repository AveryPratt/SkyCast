using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SkyCast.Startup))]
namespace SkyCast
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
