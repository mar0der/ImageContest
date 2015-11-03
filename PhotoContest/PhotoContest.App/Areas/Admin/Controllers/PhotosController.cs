namespace PhotoContest.App.Areas.Admin.Controllers
{
    #region

    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Mvc;

    using PhotoContest.App.Helpers;
    using PhotoContest.Data.Interfaces;

    #endregion

    public class PhotosController : BaseAdminController
    {
        public PhotosController(IPhotoContestData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.Redirect("/admin/photos/viewall");
        }

        public ActionResult ViewAll()
        {
            var pictures = this.Data.Photos.All();
            return this.View(pictures);
        }

        public ActionResult ForUser([FromUri] string id)
        {
            var user = this.Data.Users.Find(id);
            if (user == null)
            {
                return this.HttpNotFound();
            }

            var pictures = user.Photos;
            return this.View("~/Areas/Admin/Views/Photos/ViewAll.cshtml", pictures);
        }

        public ActionResult Delete(int id, string userId)
        {
            var user = this.Data.Users.Find(userId);
            var photo = user.Photos.FirstOrDefault(p => p.Id == id);

            if (photo == null)
            {
                return this.RedirectToAction("ViewAll", "Users");
            }

            var fileName = photo.PhotoLink.Split('/').Last().Split('?').First();

            this.Data.Photos.Delete(photo);
            this.Data.SaveChanges();

            var task = Task.Run(() => DropBoxManager.Delete(fileName));
            task.Wait();

            return this.RedirectToAction("ViewAll", "Users");
        }
    }
}