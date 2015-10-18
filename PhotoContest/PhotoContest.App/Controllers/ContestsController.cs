using System;
using System.Net;
using System.Web;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using PhotoContest.Models.Enumerations;
using PhotoContest.Models.Models;

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
        public ActionResult Add(AddContestBindingModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                throw new ArgumentException("Invalid model");
            }

            var contest = new Contest()
            {
                RewardStrategy = model.RewardStrategy,
                VotingStrategy = model.VotingStrategy,
                ParticipationStrategy = model.ParticipationStrategy,
                DeadlineStrategy = model.DeadlineStrategy,
                Description = model.Description,
                CreatedAt = DateTime.Now,
                Deadline = model.Deadline,
                MaxNumberOfParticipants = model.MaxParticipations,
                Status = ContestStatus.Active,
                Title = model.Title,
                CreatorId = this.User.Identity.GetUserId()
            };

            this.Data.Contests.Add(contest);
            this.Data.SaveChanges();

            return this.RedirectToAction("View", "Contests", new { id = contest.Id });
        }

        [Route("contests")]
        public ActionResult ViewAll()
        {
            var contests = this.Data.Contests.All().OrderBy(c => c.CreatedAt);
            return this.View(contests);
        }

        [Route("Contest/{id}")]
        public ActionResult View(int id)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == id);
            return this.View(contest);
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