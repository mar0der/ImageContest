using System;
using System.Net;
using Microsoft.Ajax.Utilities;

namespace PhotoContest.App.Controllers
{
    #region

    using System.Linq;
    using System.Web.Mvc;

    using PhotoContest.App.Models.BindingModels.Contests;
    using PhotoContest.Data.Interfaces;

    #endregion

    public class ContestsController : BaseController 
    {
        public ContestsController(IPhotoContestData data)
            : base(data)
        {
        }

        [HttpGet]
        public ActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public void Add(AddContestBindingModel model)
        {
            Console.WriteLine();
        }

        [Route("contests")]
        public ActionResult ViewAll()
        {
            var contests = this.Data.Contests.All().OrderBy(c => c.CreatedAt);
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

        public ActionResult Join()
        {
            return this.View();
        }

        public ActionResult Invite()
        {
            return this.Content("invite");
        }

        public ActionResult InviteJudge()
        {
            return this.View();
        }

        public ActionResult FinalizeContest()
        {
            return this.View();
        }
    }
}