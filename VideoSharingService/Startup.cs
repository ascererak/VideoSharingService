using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VideoSharingService.Startup))]
namespace VideoSharingService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
