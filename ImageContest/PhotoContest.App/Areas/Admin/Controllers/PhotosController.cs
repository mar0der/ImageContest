namespace PhotoContest.App.Areas.Admin.Controllers
{
    #region

    using System.Web.Mvc;

    #endregion

    public class PhotosController : Controller
    {
        public ActionResult Index()
        {
            return this.Redirect("/admin/photos/viewall");
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
    }
}