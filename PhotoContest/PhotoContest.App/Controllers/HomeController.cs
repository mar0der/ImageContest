namespace PhotoContest.App.Controllers
{
    #region

    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using PhotoContest.Data;
    using PhotoContest.Data.Interfaces;

    #endregion

    public class HomeController : BaseController
    {
        public HomeController(IPhotoContestData data)
            : base(data)
        {
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return this.RedirectToAction("ViewAll", "contests");
        }

        [AllowAnonymous]
        public ActionResult Error()
        {
            return this.View();
        }
    }
}