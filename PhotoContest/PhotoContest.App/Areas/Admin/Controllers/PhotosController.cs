using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PhotoContest.App.Helpers;
using PhotoContest.App.Models.Photos.BindingModels;
using PhotoContest.App.Models.ViewModels;
using PhotoContest.Models.Models;

namespace PhotoContest.App.Areas.Admin.Controllers
{
    #region

    using System.Web.Mvc;

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
            return this.Redirect("/admin/users/viewall");
        }

        public ActionResult ViewAll(string id)
        {
            ViewData["id"] = id;
            var user = this.Data.Users.Find(id);
            var pictures = user.Photos;
            return this.View(pictures);
        }

        public ActionResult Delete(int id, string userId)
        {
            var user = this.Data.Users.Find(userId);
            var photo = user.Photos.FirstOrDefault(p => p.Id == id);

            if (photo == null)
            {
                return this.RedirectToAction("ViewAll", "Users");
            }

            var fileName = photo.PhotoLink
                .Split('/')
                .Last()
                .Split('?')
                .First();

            this.Data.Photos.Delete(photo);
            this.Data.SaveChanges();

            var task = Task.Run(() => DropBoxManager.Delete(fileName));
            task.Wait();

            return this.RedirectToAction("ViewAll", "Users");
        }
    }
}