using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(friends_circle.Startup))]
namespace friends_circle
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
