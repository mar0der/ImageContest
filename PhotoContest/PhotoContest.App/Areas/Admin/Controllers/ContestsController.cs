namespace PhotoContest.App.Areas.Admin.Controllers
{
    #region

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;

    using Ninject.Infrastructure.Language;

    using PhotoContest.App.Models.Photos.Users;
    using PhotoContest.App.Models.ViewModels.Contests;
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models.Models;

    #endregion


    //[Route("Admin")]
    public class ContestsController : BaseAdminController
    {
        public ContestsController(IPhotoContestData data)
            : base(data)
        {
        }

        // GET: Contests
        public ActionResult Index()
        {
            return this.Redirect("/admin/contests/viewall");
        }

        public ActionResult ViewAll()
        {
            var contests = this.Data.Contests.All()
                .OrderBy(c => c.Status)
                .ThenByDescending(c => c.CreatedAt)
                .ToList();
            return this.View(contests);
        }

        public ActionResult Edit()
        {
            return this.View();
        }

        public ActionResult Delete()
        {
            return this.View();
        }

        public ActionResult Finalize()
        {
            return this.View();
        }

        public ActionResult RemovePicture()
        {
            return this.View();
        }
    }
}