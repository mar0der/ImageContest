#region

using Microsoft.Owin;

using PhotoContest.App;

#endregion

[assembly: OwinStartup(typeof(Startup))]

namespace PhotoContest.App
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