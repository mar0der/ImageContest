namespace ImageContest.App.Controllers
{
    #region

    using System.Web.Mvc;

    using PhotoContest.Data.Interfaces;

    #endregion

    public class PhotosController : BaseController
    {

        public PhotosController(IPhotoContestData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.Redirect("/Photos/View");
        }

        public ActionResult Add()
        {
            return this.View();
        }

        //change this to custom route /view and custom view template. because view is reserved word
        public ActionResult ViewPicture()
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