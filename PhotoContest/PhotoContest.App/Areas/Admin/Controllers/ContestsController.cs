using PhotoContest.Models.Enumerations;
using WebGrease.Css.Extensions;

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

        [ValidateAntiForgeryToken]
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


        [ValidateAntiForgeryToken]
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

        [System.Web.Mvc.HttpGet]
        public ActionResult Finalize(int id)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == id);
            var contestViewModel = Mapper.Map<Contest, ContestViewModel>(contest);
            if (contest == null)
            {
                return this.RedirectToAction("ViewAll", "Contests");
            }
            return this.View(contestViewModel);
        }

        [ValidateAntiForgeryToken]
        [System.Web.Mvc.HttpPost]
        public ActionResult Finalize(int? id)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == id);

            if (contest == null)
            {
                throw new ApplicationException("Invalid contest id");
            }

            contest.Status = ContestStatus.Ended;

            var winners = new Dictionary<Places, Photo>();

            if (contest.RewardStrategy == RewardStrategy.SingleWinner)
            {
                winners = this.PickWinners(contest);
            }
            else if (contest.RewardStrategy == RewardStrategy.Top3Prizes)
            {
                winners = this.PickWinners(contest, 5);
            }

            var contestWinners = new List<ContestWinner>();
            winners.ForEach(winner =>
            {
                contestWinners.Add(new ContestWinner()
                {
                    Contest = contest,
                    PhotoId = winner.Value.Id,
                    Place = winner.Key,
                });

            });

            contest.ContestWinners = contestWinners;
            this.Data.SaveChanges();

            return this.RedirectToAction("ViewAll", "Contests", new { area = "Admin" });
        }

        private Dictionary<Places, Photo> PickWinners(Contest contest, int winnersCount = 1)
        {
            var averageVoteCount = contest.Photos.Average(p => p.Votes.Count());
            var winners =
                contest.Photos.Where(p => p.Votes.Count() >= averageVoteCount)
                    .OrderByDescending(p => p.Votes.Average(v => v.Stars))
                    .Take(winnersCount)
                    .ToList();

            var winnersDict = new Dictionary<Places, Photo>();
            if (winners.Count() >= 1)
            {
                winnersDict.Add(Places.Gold, winners[0]);
            }
            if (winners.Count() >= 2)
            {
                winnersDict.Add(Places.Silver, winners[1]);
            }
            if (winners.Count() >= 3)
            {
                winnersDict.Add(Places.Bronze, winners[2]);
            }

            return winnersDict;
        }


        public ActionResult RemovePicture()
        {
            return this.View();
        }
    }
}