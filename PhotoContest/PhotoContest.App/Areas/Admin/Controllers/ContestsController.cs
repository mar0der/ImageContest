namespace PhotoContest.App.Areas.Admin.Controllers
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Mvc;
    

    using AutoMapper;

    using Microsoft.AspNet.Identity;

    using Ninject.Infrastructure.Language;

    using PhotoContest.App.Models.Photos.Contests;
    using PhotoContest.App.Models.Photos.Users;
    using PhotoContest.App.Models.ViewModels.Contests;
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models.Models;
    

    #endregion

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

        public ActionResult Edit(int? id)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == id);
            if (contest == null)
            {
                return this.RedirectToAction("ViewAll", "Contests");
            }
            return this.View(contest);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult SaveContest(ContestBindingModel updatedContest)
        {
            if (!this.ModelState.IsValid || updatedContest == null)
            {
                throw new ArgumentException("Invalid model");
            }

            var contest = this.Data.Contests.All().FirstOrDefault(a => a.Id == updatedContest.Id);

            if (contest == null)
            {
                return this.HttpNotFound("No such contest");
            }

            contest.Description = updatedContest.Description;
            contest.MaxNumberOfParticipants = updatedContest.MaxNumberOfParticipants;
            contest.Status = updatedContest.Status;
            contest.Title = updatedContest.Title;
            contest.RewardStrategy = updatedContest.RewardStrategy;
            contest.Deadline = updatedContest.Deadline;
            contest.DeadlineStrategy = updatedContest.DeadlineStrategy;
            contest.VotingStrategy = updatedContest.VotingStrategy;

            this.Data.SaveChanges();

            return this.RedirectToAction("ViewAll", "Contests", new { area = "Admin" });
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Delete(int id)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == id);
            var contestViewModel = Mapper.Map<Contest, ContestViewModel>(contest);
            if (contest == null)
            {
                return this.RedirectToAction("ViewAll", "Contests");
            }
            return this.View(contestViewModel);
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult Delete([FromBody] int? id)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == id);

            if (contest == null)
            {
                throw new ApplicationException("Invalid contest id");
            }

            this.Data.Contests.Delete(contest);
            this.Data.SaveChanges();

            return this.RedirectToAction("ViewAll", "Contests", new {area = "Admin"});
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