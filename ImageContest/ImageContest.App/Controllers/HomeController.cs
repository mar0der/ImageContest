namespace ImageContest.App.Controllers
{
    #region

    using System.Linq;
    using System.Web.Mvc;

    using Data;
    using Data.Repositories;

    using ImageContest.Data.Interfaces;

    #endregion

    public class HomeController : BaseController
    {
        public HomeController(IImageContestData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            var users = this.Data.Users.All().ToList();
            return this.View();
        }

        public ActionResult About()
        {
            this.ViewBag.Message = "Your application description page.";

            return this.View();
        }

        public ActionResult Contact()
        {
            this.ViewBag.Message = "Your contact page.";

            return this.View();
        }
    }
}