#region

using Microsoft.Owin;

using PhotoContest.App;

#endregion

[assembly: OwinStartup(typeof(Startup))]

namespace PhotoContest.App
{
    #region

    using Microsoft.AspNet.SignalR;
    using Owin;

    #endregion

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr",
                map =>
                {
                    map.RunSignalR(new HubConfiguration());
                });
            this.ConfigureAuth(app);
        }
    }
}