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
    using PhotoContest.App.Jobs;

    #endregion

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JobScheduler.StartFinilizeContestJob();
            app.Map("/signalr",
                map =>
                {
                    map.RunSignalR(new HubConfiguration());
                });
            app.MapSignalR();
            this.ConfigureAuth(app);
        }
    }
}