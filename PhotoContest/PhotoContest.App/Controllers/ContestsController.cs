using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.WebPages;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PhotoContest.Models.Enumerations;
using PhotoContest.Models.Models;
using WebGrease.Css.Extensions;

namespace PhotoContest.App.Controllers
{
    #region

    using System.Linq;
    using System.Web.Mvc;

    using PhotoContest.App.Models.Photos.Contests;
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
            if (contest == null)
            {
                return this.RedirectToAction("ViewAll", "Contests");
            }
            return this.View(contest);
        }

        [Route("Contest/Edit/{id}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == id);
            if (contest == null)
            {
                return this.RedirectToAction("ViewAll", "Contests");
            }
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
        [HttpGet]
        [Route("Contests/Delete/{id}")]
        public ActionResult Delete(int id)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == id);
            if (contest == null)
            {
                return this.RedirectToAction("ViewAll", "Contests");
            }
            return this.View(contest);
        }

        [HttpPost]
        [Route("Contests/Delete/{id}")]
        public ActionResult DeleteContest(int id)
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

            if (contest.ParticipationStrategy != ParticipationStrategy.Open)
            {
                throw new ApplicationException("You should be invited to this contest");
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

        [HttpGet]
        public ActionResult Invite()
        {
            return this.View();
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

        // TODO: adding binding model and change to httpPost
        // This action did not work, because there are no photos
        [HttpGet]
        [Route("Contest/Vote/{stars}/{photoId}/{contestId}")]
        public void Vote(int stars, int photoId, int contestId)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == contestId);

            if (contest == null)
            {
                throw new ApplicationException("Invalid contest id");
            }

            var userId = this.User.Identity.GetUserId();
            var user = this.Data.Users.All().Single(u => u.Id == userId);

            if (!contest.Participants.Contains(user))
            {
                throw new ApplicationException("You are not participant to this contest");
            }

            var vote = new Vote()
            {
                Contest = contest,
                PhotoId = photoId,
                Stars = stars,
                User = user
            };

            contest.Votes.Add(vote);
            this.Data.SaveChanges();

            // TODO: redirect or message for successfuly voting
        }

        public ActionResult FinalizeContest()
        {
            return this.View();
        }

        [HttpGet]
        [Route("Contests/SearchForUser/{user}")]
        public string SearchForUser(string user)
        {
            var users = this.Data.Users.All().Where(u => u.UserName.Contains(user));
            var userList = new List<User>();
            if (users.Any())
            {
                users.ForEach(u =>
                {
                    var newUser = new User()
                    {
                        Id = u.Id,
                        Username = u.UserName
                    };

                    userList.Add(newUser);
                });
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(userList);
        }

    }
}