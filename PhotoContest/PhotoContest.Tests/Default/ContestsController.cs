using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PhotoContest.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_AddValidContest_ShoudBeAddedInDB()
        {

        }

        [TestMethod]
        public void Test_AddInvalidContest_ShoudReturnError()
        {

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
        public void Test_InviteExsistingUserToBeJudgeToExsistingContestWithCloseStrategy_ShoudJoinUserToBeJudgeToContest()
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


    }
}
