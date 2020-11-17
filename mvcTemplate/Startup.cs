using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Realtek.IntraLogin.Startup))]
namespace Realtek.IntraLogin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
