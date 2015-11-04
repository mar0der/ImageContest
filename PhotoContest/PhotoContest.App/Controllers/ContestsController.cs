namespace PhotoContest.App.Controllers
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    using AutoMapper;

    using Microsoft.AspNet.Identity;

    using PhotoContest.App.Models.BindingModels.Contests;
    using PhotoContest.App.Models.Photos.Contests;
    using PhotoContest.App.Models.ViewModels.Contests;
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models.Enumerations;
    using PhotoContest.Models.Models;

    using WebGrease.Css.Extensions;

    using User = PhotoContest.App.Models.Photos.Contests.User;

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
            if (!this.ModelState.IsValid || model == null)
            {
                this.ModelState.AddModelError(string.Empty, "Invalid model");
            }

            if (model != null && model.Deadline <= DateTime.Now)
            {
                this.ModelState.AddModelError("Deadline", "Invalid deadline");
            }

            if (this.ModelState.IsValid)
            {
                var contest = Mapper.Map<ContestBindingModel, Contest>(model);
                contest.CreatedAt = DateTime.Now;
                contest.CreatorId = this.User.Identity.GetUserId();

                this.Data.Contests.Add(contest);
                this.Data.SaveChanges();

                return this.RedirectToAction("View", "Contests", new { id = contest.Id });
            }

            return this.View();
        }

        [AllowAnonymous]
        [Route("contests")]
        public ActionResult ViewAll()
        {
            var contests = this.Data.Contests.All().OrderBy(c => c.CreatedAt).ToList();
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
            this.CheckContestDeadline(contest);
            return this.View(contest);
        }

        private void CheckContestDeadline(Contest contest)
        {
            if (contest.Deadline < DateTime.Now && contest.Status == ContestStatus.Active)
            {
                contest.Status = ContestStatus.Ended;
                this.Data.SaveChanges();
            }
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
        public ActionResult EditContest(ContestBindingModel updatedContest)
        {
            if (!this.ModelState.IsValid || updatedContest == null)
            {
                throw new ArgumentException("Invalid model");
            }

            var contest = this.Data.Contests.All().First(a => a.Id == updatedContest.Id);

            contest.Description = updatedContest.Description;
            contest.MaxNumberOfParticipants = updatedContest.MaxNumberOfParticipants;
            contest.Status = updatedContest.Status;
            contest.Title = updatedContest.Title;
            contest.RewardStrategy = updatedContest.RewardStrategy;
            contest.Deadline = updatedContest.Deadline;
            contest.DeadlineStrategy = updatedContest.DeadlineStrategy;
            contest.VotingStrategy = updatedContest.VotingStrategy;

            this.Data.SaveChanges();

            return this.RedirectToAction("View", "Contests", new { id = contest.Id });
        }

        [HttpGet]
        [Route("Contests/Delete/{id}")]
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

            var userId = this.User.Identity.GetUserId();

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
                this.ModelState.AddModelError(string.Empty, "Invalid contest id");
            }

            var user = this.Data.Users.All().SingleOrDefault(u => u.UserName == username);
            if (user == null)
            {
                this.ModelState.AddModelError(string.Empty, "Invalid user id");
            }

            if (contest.CreatorId != this.User.Identity.GetUserId())
            {
                this.ModelState.AddModelError(string.Empty, "You are not owner of the contest");
            }

            if (contest.CreatorId == user.Id)
            {
                this.ModelState.AddModelError(string.Empty, "Invalid invitation");
            }

            if (contest.Participants.Contains(user))
            {
                this.ModelState.AddModelError(string.Empty, "The user already participants in the contest");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            contest.Participants.Add(user);
            this.Data.SaveChanges();

            return this.RedirectToAction("View", "Contests", new { id = contest.Id });
        }

        [HttpGet]
        public ActionResult InviteJudge()
        {
            return this.View();
        }

        [HttpGet]
        [Route("Contests/InviteJudge/{constestId}/{username}")]
        public ActionResult InviteJudge(int constestId, string username)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == constestId);
            if (contest == null)
            {
                this.ModelState.AddModelError(string.Empty, "Invalid contest id");
            }

            var user = this.Data.Users.All().SingleOrDefault(u => u.UserName == username);
            if (user == null)
            {
                this.ModelState.AddModelError(string.Empty, "Invalid user id");
            }
            else
            {
                if (user.Id == this.CurrentUser.Id)
                {
                    this.ModelState.AddModelError(string.Empty, "You cannot invite youself as judge");
                }
            }

            if (contest != null)
            {
                if (contest.CreatorId != this.User.Identity.GetUserId())
                {
                    this.ModelState.AddModelError(string.Empty, "You are not owner of the contest");
                }

                if (contest.Judges.Contains(user))
                {
                    this.ModelState.AddModelError(string.Empty, "The user is already a judge");
                }

                if (contest.VotingStrategy != VotingStrategy.Closed)
                {
                    this.ModelState.AddModelError(string.Empty, "You cannot invite judges, because the voting strategy is open.");
                }
            }

            if (this.ModelState.IsValid)
            {
                contest.Judges.Add(user);
                this.Data.SaveChanges();

                return this.RedirectToAction("View", "Contests", new { id = contest.Id });
            }

            return this.View(this.ModelState);
        }

        [HttpPost]
        public ActionResult Vote(VoteBindingModel model)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == model.ContestId);
            var user = this.CurrentUser;
            if (contest == null)
            {
                this.ModelState.AddModelError(string.Empty, "Invalid contest id");
            }
            else
            {
                if (!contest.Participants.Contains(user))
                {
                    this.ModelState.AddModelError(string.Empty, "You are not participant to this contest");
                }

                if (contest.Votes.Any(v => v.User == user))
                {
                    this.ModelState.AddModelError(string.Empty, "You already vote for this picture");
                }

                if (contest.VotingStrategy == VotingStrategy.Closed)
                {
                    if (!contest.Judges.Contains(user))
                    {
                        this.ModelState.AddModelError(string.Empty, "You are not judge");
                    }
                }
            }

            if (this.ModelState.IsValid)
            {
                var vote = new Vote { Contest = contest, PhotoId = model.PhotoId, Stars = model.Stars, User = user };

                contest.Votes.Add(vote);
                this.Data.SaveChanges();

                var voteAvg = string.Format("{0:F2}", contest.Votes.Average(v => v.Stars));
                return this.Content(voteAvg);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid voting data");
        }

        [Route("Contests/FinalizeContest/{contestId}")]
        public ActionResult FinalizeContest(int contestId)
        {
            var contest = this.Data.Contests.All().SingleOrDefault(c => c.Id == contestId);

            if (contest == null)
            {
                throw new ApplicationException("Invalid contest id");
            }

            if (contest.CreatorId != this.CurrentUser.Id)
            {
                throw new ApplicationException("You cannot finalize contest");
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

            return this.View();
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

        [HttpGet]
        [Route("Contests/SearchForUser/{user}")]
        public string SearchForUser(string user)
        {
            var users = this.Data.Users.All().Where(u => u.UserName.Contains(user));
            var userList = new List<User>();
            if (users.Any())
            {
                users.ForEach(
                    u =>
                        {
                            var newUser = new User { Id = u.Id, Username = u.UserName };

                            userList.Add(newUser);
                        });
            }

            var serializer = new JavaScriptSerializer();
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
                this.ModelState.AddModelError(string.Empty, "Invalid contest id");
            }

            var photo = this.CurrentUser.Photos.SingleOrDefault(p => p.Id == photoId);

            if (photo == null)
            {
                this.ModelState.AddModelError(string.Empty, "Invalid photo id");
            }

            if (contest != null)
            {
                if (!contest.Participants.Contains(this.CurrentUser) && contest.CreatorId != this.User.Identity.GetUserId())
                {
                    return this.RedirectToAction("Index", "Home");
                }

                if (contest.Status == ContestStatus.Ended)
                {
                    this.ModelState.AddModelError(string.Empty, "The contests is closed");
                }

                if (contest.Photos.Contains(photo))
                {
                    this.ModelState.AddModelError(string.Empty, "The photo is already in the contest");
                }
            }

            if (!this.ModelState.IsValid)
            {
                var photos = this.CurrentUser.Photos;
                return this.View(photos);
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