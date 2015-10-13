#region

using ImageContest.App;

using Microsoft.Owin;

#endregion

[assembly: OwinStartup(typeof(Startup))]

namespace ImageContest.App
{
    #region

    using Owin;

    #endregion

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}