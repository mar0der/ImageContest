namespace ImageContest.App.Controllers
{
    #region

    using System;
    using System.Web.Mvc;

    using PhotoContest.Data.Interfaces;

    #endregion

    public class ContestsController : BaseController
    {
        public ContestsController(IPhotoContestData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.Redirect("/contests/viewall");
        }

        public ActionResult Add()
        {
            return this.View();
        }

        //add custom url and view template
        public ActionResult ViewAll()
        {
            return this.View();
        }

        public ActionResult Edit()
        {
            return this.View();
        }

        public ActionResult Delete()
        {
            return this.View();
        }

        public ActionResult Join()
        {
            return this.View();
        }

        //this should not return result. Json maybe
        public ActionResult Invite()
        {
            return this.HttpNotFound();
        }

        public ActionResult InviteJudge()
        {
            return this.View();
        }

        public ActionResult Finalize()
        {
            return this.View();
        }


    }
}