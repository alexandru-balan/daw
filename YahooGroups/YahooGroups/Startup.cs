using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YahooGroups.Startup))]
namespace YahooGroups
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
