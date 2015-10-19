using System;
using System.Net;
using System.Web;
using System.Web.Http;
using AutoMapper;
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
        public ActionResult Add(ContestBindingModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                throw new ArgumentException("Invalid model");
            }

            var contest = Mapper.Map<ContestBindingModel, Contest>(model);
            contest.CreatedAt = DateTime.Now;
            contest.CreatorId = this.User.Identity.GetUserId();

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

        [Route("Contest/Edit/{id}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == id);
            return this.View(contest);
        }

        [HttpPost]
        [Route("Contest/Edit")]
        public ActionResult EditContest(ContestBindingModel contest)
        {
            if (!ModelState.IsValid || contest == null)
            {
                throw new ArgumentException("Invalid model");
            }

            var contestDb = this.Data.Contests.All().First(a => a.Id == contest.Id);
            // With mapper did not work database save
            //contestDb = Mapper.Map<ContestBindingModel, Contest>(contest);

            contestDb.Description = contest.Description;
            contestDb.MaxNumberOfParticipants = contest.MaxNumberOfParticipants;
            contestDb.Status = contest.Status;
            contestDb.Title = contest.Title;
            contestDb.RewardStrategy = contest.RewardStrategy;
            contestDb.Deadline = contest.Deadline;
            contestDb.DeadlineStrategy = contest.DeadlineStrategy;
            contestDb.VotingStrategy = contest.VotingStrategy;

            this.Data.SaveChanges();

            return this.RedirectToAction("View", "Contests", new { id = contestDb.Id });
        }

        [HttpPost]
        [Route("Contests/Delete/{id}")]
        public ActionResult Delete(int id)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == id);

            if (contest == null)
            {
                throw new ApplicationException("Invalid contest id");
            }

            if (contest.Creator.Id != this.User.Identity.GetUserId())
            {
                throw new ApplicationException("You are not the owner of the contest");
            }

            this.Data.Contests.Delete(contest);
            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Join(int id)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == id);

            if (contest == null)
            {
                throw new ApplicationException("Invalid contest id");
            }

            string userId = this.User.Identity.GetUserId();

            if (contest.Participants.Any(p => p.Id == userId))
            {
                throw new ApplicationException("You are already a member of this contest");
            }

            var user = this.Data.Users.All().First(u => u.Id == userId);

            contest.Participants.Add(user);
            this.Data.SaveChanges();

            return this.RedirectToAction("View", "Contests", new { id = contest.Id});
        }

        [Route("Contests/Invite/{username}/{contestId}")]
        public ActionResult Invite(string username, int contestId)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == contestId);
            if (contest == null)
            {
                throw new ApplicationException("Invalid contest id");
            }

            var user = this.Data.Users.All().SingleOrDefault(u => u.UserName == username);
            if (user == null)
            {
                throw new ApplicationException("Invalid user id");
            }

            if (contest.CreatorId != this.User.Identity.GetUserId())
            {
                throw new ApplicationException("You are not owner of the contest");
            }

            if (contest.Participants.Contains(user))
            {
                throw new ApplicationException("The user already participants in the contest");
            }

            contest.Participants.Add(user);
            this.Data.SaveChanges();

            return this.RedirectToAction("View", "Contests", new { id = contest.Id });
        }

        [Route("Contests/InviteJudge/{username}/{contestId}")]
        public ActionResult InviteJudge(string username, int contestId)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == contestId);
            if (contest == null)
            {
                throw new ApplicationException("Invalid contest id");
            }

            var user = this.Data.Users.All().SingleOrDefault(u => u.UserName == username);
            if (user == null)
            {
                throw new ApplicationException("Invalid user id");
            }

            if (contest.CreatorId != this.User.Identity.GetUserId())
            {
                throw new ApplicationException("You are not owner of the contest");
            }

            if (contest.Judges.Contains(user))
            {
                throw new ApplicationException("The user is already a judge");
            }

            contest.Judges.Add(user);
            this.Data.SaveChanges();

            return this.RedirectToAction("View", "Contests", new { id = contest.Id });
        }

        public ActionResult FinalizeContest()
        {
            return this.View();
        }
    }
}