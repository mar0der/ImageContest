namespace PhotoContest.App.Areas.Admin.Controllers
{
    #region

    using System.Web.Mvc;
    using System.Web.Security;

    using PhotoContest.Data.Interfaces;

    #endregion

    public class UsersController : BaseAdminController
    {
        public UsersController(IPhotoContestData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.Redirect("/admin/users/viewall");
        }

        public ActionResult ViewAll()
        {

            return this.View();
        }

        public ActionResult Delete()
        {
            return this.View();
        }

        public ActionResult Edit()
        {
            return this.View();
        }
    }
}