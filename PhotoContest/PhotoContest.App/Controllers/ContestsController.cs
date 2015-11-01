using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Script.Serialization;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PhotoContest.App.Models.BindingModels.Contests;
using PhotoContest.Models.Enumerations;
using PhotoContest.Models.Models;
using WebGrease.Css.Extensions;
using PhotoContest.Models.Models;

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
                ModelState.AddModelError(string.Empty, "Invalid model");
            }

            if (model != null && model.Deadline <= DateTime.Now)
            {
                ModelState.AddModelError("Deadline", "Invalid deadline");
            }

            if (ModelState.IsValid)
            {
                var contest = Mapper.Map<ContestBindingModel, Contest>(model);
                contest.CreatedAt = DateTime.Now;
                contest.CreatorId = this.User.Identity.GetUserId();

                this.Data.Contests.Add(contest);
                this.Data.SaveChanges();

                return this.RedirectToAction("View", "Contests", new { id = contest.Id });
            }

            return View();
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
                throw new ApplicationException("The participation feature is closed");
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

            return this.RedirectToAction("View", "Contests", new { id = contest.Id });
        }

        [HttpGet]
        public ActionResult Invite()
        {
            return this.View();
        }

        [Route("Contests/Invite/{contestId}/{username}")]
        public ActionResult Invite(int contestId, string username)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == contestId);
            if (contest == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid contest id");
            }

            var user = this.Data.Users.All().SingleOrDefault(u => u.UserName == username);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid user id");
            }

            if (contest.CreatorId != this.User.Identity.GetUserId())
            {
                ModelState.AddModelError(string.Empty, "You are not owner of the contest");
            }

            if (contest.CreatorId == user.Id)
            {
                ModelState.AddModelError(string.Empty, "Invalid invitation");
            }

            if (contest.Participants.Contains(user))
            {
                ModelState.AddModelError(string.Empty, "The user already participants in the contest");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            contest.Participants.Add(user);
            this.Data.SaveChanges();

            return this.RedirectToAction("View", "Contests", new { id = contest.Id });
        }


        [HttpGet]
        public ActionResult InviteJudge()
        {
            return View();
        }

        [HttpGet]
        [Route("Contests/InviteJudge/{constestId}/{username}")]
        public ActionResult InviteJudge(int constestId, string username)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == constestId);
            if (contest == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid contest id");
            }

            var user = this.Data.Users.All().SingleOrDefault(u => u.UserName == username);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid user id");
            }
            else
            {
                if (user.Id == this.CurrentUser.Id)
                {
                    ModelState.AddModelError(string.Empty, "You cannot invite youself as judge");
                }
                
            }

            if (contest != null)
            {
                if (contest.CreatorId != this.User.Identity.GetUserId())
                {
                    ModelState.AddModelError(string.Empty, "You are not owner of the contest");
                }

                if (contest.Judges.Contains(user))
                {
                    ModelState.AddModelError(string.Empty, "The user is already a judge");
                } 
            }

            if (ModelState.IsValid)
            {
                contest.Judges.Add(user);
                this.Data.SaveChanges();

                return this.RedirectToAction("View", "Contests", new {id = contest.Id});

            }

            return View(ModelState);
        }

        [HttpPost]
        public ActionResult Vote(VoteBindingModel model)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == model.ContestId);
            var user = this.CurrentUser;
            if (contest == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid contest id");
            }
            else
            {
                if (!contest.Participants.Contains(user))
                {
                    ModelState.AddModelError(String.Empty, "You are not participant to this contest");
                }

                if (contest.Votes.Any(v => v.User == user))
                {
                    ModelState.AddModelError(string.Empty, "You already vote for this picture");
                }
            }

            if (ModelState.IsValid)
            {
                var vote = new Vote()
                {
                    Contest = contest,
                    PhotoId = model.PhotoId,
                    Stars = model.Stars,
                    User = user
                };

                contest.Votes.Add(vote);
                this.Data.SaveChanges();

                var voteAvg = string.Format("{0:F2}", contest.Votes.Average(v => v.Stars));
                return this.Content(voteAvg);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid voting data");
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

        [HttpGet]
        public ActionResult AddPhoto()
        {
            var photos = this.CurrentUser.Photos;

            return this.View(photos);
        }

        [HttpGet]
        [Route("Contests/AddPhoto/{contestId}/{photoId}")]
        public ActionResult AddPhoto(int contestId, int photoId)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == contestId);

            if (contest == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid contest id");
            }

            var photo = this.CurrentUser.Photos.SingleOrDefault(p => p.Id == photoId);

            if (photo == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid photo id");
            }

            if (contest != null && !contest.Participants.Contains(this.CurrentUser))
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (contest != null && contest.Photos.Contains(photo))
            {
                ModelState.AddModelError(string.Empty, "The photo is already in the contest");
            }

            if (!ModelState.IsValid)
            {
                var photos = this.CurrentUser.Photos;
                return View(photos);
            }

            contest.Photos.Add(photo);
            this.Data.SaveChanges();
            return this.RedirectToAction("View", "Contests", new { id = contest.Id });
        }

        [Route("Contests/ViewPhoto/{contestId}/{photoId}")]
        public ActionResult ViewPhoto(int contestId, int photoId)
        {
            var contest = this.Data.Contests.Find(contestId);
            if (contest == null)
            {
                return this.RedirectToAction("ViewAll", "Contests");
            }

            var photo = contest.Photos.FirstOrDefault(p => p.Id == photoId);
            if (photo == null)
            {
                return this.RedirectToAction("View", "Contests", new { id = contestId });
            }

            var viewModel = Mapper.Map<Photo, ViewPhotoModel>(photo);
            viewModel.ContestTitle = contest.Title;
            viewModel.ContestId = contest.Id;

            return this.View(viewModel);
        }
    }
}