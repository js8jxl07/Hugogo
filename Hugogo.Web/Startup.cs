using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hugogo.Web.Startup))]
namespace Hugogo.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
