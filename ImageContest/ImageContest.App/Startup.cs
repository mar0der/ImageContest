using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImageContest.App.Startup))]
namespace ImageContest.App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
