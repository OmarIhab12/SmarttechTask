using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SmarttechTask.Startup))]
namespace SmarttechTask
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
