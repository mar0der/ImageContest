namespace PhotoContest.App.Controllers
{
    #region

    using System.Web.Mvc;

    using PhotoContest.Data.Interfaces;

    #endregion

    public class UsersController : BaseController
    {
        public UsersController(IPhotoContestData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.Redirect("/users/profile");
        }

        public ActionResult Profile()
        {
            return this.View();
        }
    }
}