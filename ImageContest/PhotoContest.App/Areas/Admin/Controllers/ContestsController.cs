namespace PhotoContest.App.Areas.Admin.Controllers
{
    #region

    using System.Web.Mvc;

    #endregion

    public class ContestsController : Controller
    {
        // GET: Contests
        public ActionResult Index()
        {
            return this.Redirect("/admin/contests/viewall");
        }

        public ActionResult ViewAll()
        {
            return this.View();
        }

        public ActionResult Edit()
        {
            return this.View();
        }

        public ActionResult Delete()
        {
            return this.View();
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