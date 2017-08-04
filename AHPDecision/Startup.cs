using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AHPDecision.Startup))]
namespace AHPDecision
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
