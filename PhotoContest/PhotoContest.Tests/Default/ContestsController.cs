namespace PhotoContest.Tests.Default
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using AutoMapper;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using PhotoContest.App.Controllers;
    using PhotoContest.App.Models.Photos.Contests;
    using PhotoContest.Data;
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Data.Repositories;
    using PhotoContest.Models.Enumerations;
    using PhotoContest.Models.Models;

    using User = PhotoContest.Models.Models.User;

    #endregion

    [TestClass]
    public class UnitTest1
    {
        private PhotoContestDbContext context;

        private ContestsController controller;

        private User user;

        private int saveChangesCount = 0;

        private List<Contest> contests = new List<Contest>();

        private Mock<IPhotoContestData> repo;

        public PhotoContestData Data { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            Mapper.CreateMap<ContestBindingModel, Contest>();
            this.user = new User()
                            {
                                Id = "1",
                                UserName = "mar0der"
                            };


            this.repo = new Mock<IPhotoContestData>();
            this.repo.Setup(r => r.Contests.Add(It.IsAny<Contest>())).Callback<Contest>(c => this.contests.Add(c));
            this.repo.Setup(r => r.SaveChanges()).Callback(this.AddSaveCount);
            this.controller = new ContestsController(this.repo.Object, this.user);

        }

        private void AddSaveCount()
        {
            this.saveChangesCount++;
        }

        [TestMethod]
        public void Test_AddValidContest_ShoudBeAddedInDB()
        {
            //arange
            var newContest = new ContestBindingModel
                                 {
                                     Title = "Test Contest",
                                     CreatedAt = DateTime.Now,
                                     Deadline = DateTime.Now.AddDays(1),
                                     MaxNumberOfParticipants = 12,
                                     Description = "mega qk contest",
                                     Status = ContestStatus.Active,
                                     RewardStrategy = RewardStrategy.SingleWinner,
                                     VotingStrategy = VotingStrategy.Open,
                                     ParticipationStrategy = ParticipationStrategy.Open,
                                     DeadlineStrategy = DeadlineStrategy.ByTime
                                     
                                 };
            //act
            this.controller.Add(newContest);
            //assert
            Assert.AreEqual(1, this.contests.Count);
            Assert.AreEqual(1, this.saveChangesCount);
            Assert.AreEqual("Test Contest", this.contests[0].Title);
            
        }

        [TestMethod]
        public void Test_AddInvalidContest_ShoudReturnError()
        {
            var date = DateTime.Now;
            var wrongContest = new ContestBindingModel
            {
                Title = "Test Contest",
                CreatedAt = date,
                Deadline = date,
                MaxNumberOfParticipants = 12,
                Description = "mega qk contest",
                Status = ContestStatus.Active,
                RewardStrategy = RewardStrategy.SingleWinner,
                VotingStrategy = VotingStrategy.Open,
                ParticipationStrategy = ParticipationStrategy.Open,
                DeadlineStrategy = DeadlineStrategy.ByTime

            };

            // Act.
            var viewResult = this.controller.Add(wrongContest) as ViewResult;

            // Assert.
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewData.ModelState["Deadline"]);
            Assert.IsNotNull(viewResult.ViewData.ModelState["Deadline"].Errors);
            Assert.IsTrue(viewResult.ViewData.ModelState["Deadline"].Errors.Count == 1);
        }

        [TestMethod]
        public void Test_AddNull_ShoudReturnError()
        {
            // Act.
            var viewResult = this.controller.Add(null) as ViewResult;

            // Assert.
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewData.ModelState[string.Empty]);
            Assert.IsNotNull(viewResult.ViewData.ModelState[string.Empty].Errors);
            Assert.IsTrue(viewResult.ViewData.ModelState[string.Empty].Errors.Count == 1);
        }

        [TestMethod]
        public void Test_EditExistingContestWithValidData_ShoudUpdateDataInDB()
        {
        }

        [TestMethod]
        public void Test_EditExistingContestWithInvalidData_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_EditUnexistantContest_ShoudRedirect()
        {
        }

        [TestMethod]
        public void Test_DeleteExsistngContest_ShoudDeleteContestInDB()
        {
        }

        [TestMethod]
        public void Test_DeleteUnexsistngContest_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_JoinExsistingContestForFirstTimeWithOpenStrategy_ShoudJoinInContest()
        {
        }

        [TestMethod]
        public void Test_JoinExsistingContestForSecoundTimeWithOpenStrategy_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_JoinExsistingContestWithCloseStrategy_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_JoinUnexsistingContest_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_InviteExsistingUserToExsistingContest_ShoudJoinUserToContest()
        {
        }

        [TestMethod]
        public void Test_InviteUnexsistingUserToExsistingContest_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_InviteExsistingUserToUnexsistingContest_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_InviteEistingUserWhoIsAlreadyJoinedExsistingContest_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_InviteExsistingUserToBeJudgeToExsistingContestWithCloseStrategy_ShoudJoinUserToBeJudgeToContest
            ()
        {
        }

        [TestMethod]
        public void Test_InviteExsistingUserToBeJudgeToExsistingContestWithOpenStrategy_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_InviteUnexsistingUserToBeJudgeToExsistingContest_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_InviteExsistingUserToBeJudgeToUnexsistingContest_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_InviteYourselfToBeJudgeToExsistingContest_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_InviteExsitingUserWhoIsAlreadyJudgeToExsistingContest_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_AddingPhotoToExistingContestWhichYouAreJoined_ShoudAddPhotoToContest()
        {
        }

        [TestMethod]
        public void Test_AddingPhotoToExistingContestWhichYouAreNotJoined_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_AddingPhotoToUnexistingContest_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_AddingUnexsistingPhotoToExistingContest_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_AddingPhotoThatAlreadyExistToExistingContest_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_VoteWithValidDataInExsistingContestsYourAreJoinOrBeingJudge_ShoudUpdateVotesInDB()
        {
        }

        [TestMethod]
        public void Test_VoteWithValidDataInUnexsistingContests_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_VoteWithInvalidDataInExsistingContests_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_VoteWithValidDataInExsistingContestsWithCloseStrategyButYouAreNotJudge_ShoudReturnException()
        {
        }

        [TestMethod]
        public void Test_VoteWithValidDataInExsistingContestsForYourPicture_ShoudReturnException()
        {
        }

        private void CreateUser(string username, string role = null)
        {
            var userManager = new UserManager<User>(new UserStore<User>(this.context));
            if (userManager.FindByName(username) == null)
            {
                var user = new User
                               {
                                   UserName = username,
                                   Email = username + "@gmail.com",
                                   PasswordHash =
                                       "ADDqeu799LPu2MFv/G9l9Dc3W5aM60JfeYUQx8JzZIkXL+IJ0SVahuH+m6/3efWFqw=="

                                   // pass is 123
                               };

                var result = userManager.Create(user);

                if (result.Succeeded && role != null)
                {
                    userManager.AddToRole(user.Id, role);
                }
            }
        }

        private void CreateRole(string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(this.context));
            if (!roleManager.RoleExists(roleName))
            {
                var role = new IdentityRole();
                role.Name = roleName;
                roleManager.Create(role);
            }
        }
    }
}