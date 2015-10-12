#region

using ImageContest.App;

using Microsoft.Owin;

#endregion

[assembly: OwinStartup(typeof(Startup))]

namespace ImageContest.App
{
    #region

    using System.Reflection;
    using System.Web.Http;

    using ImageContest.Data;
    using ImageContest.Data.Interfaces;
    using ImageContest.Data.Repositories;

    using Ninject;
    using Ninject.Web.Common.OwinHost;
    using Ninject.Web.WebApi.OwinHost;

    using Owin;

    #endregion

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
            //app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(GlobalConfiguration.Configuration);
        }

        private static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            RegisterMapping(kernel);
            return kernel;
        }

        private static void RegisterMapping(StandardKernel kernel)
        {
            //kernel.Bind<IImageContestData>().To<ImageContestData>().WithConstructorArgument("context", c => new ImageContextDbContext());
        }
    }
}