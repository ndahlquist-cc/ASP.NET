using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NDSailing.Startup))]
namespace NDSailing
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
