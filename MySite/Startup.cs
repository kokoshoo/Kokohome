using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MySite.Startup))]
namespace MySite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
