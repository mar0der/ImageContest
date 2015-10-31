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

        [HttpGet]
        public ActionResult Edit(int id, string userId)
        {
            ViewData["id"] = userId;
            var photo = this.Data.Photos.Find(id);
            if (photo == null)
            {
                return this.RedirectToAction("ViewAll", "Users");
            }

            var photoViewModel = Mapper.Map<Photo, EditPhotoModel>(photo);
            return this.View(photoViewModel);
        }

        [HttpPost]
        public ActionResult Edit(string id, EditPhotoModel model)
        {
            if (model == null || !this.ModelState.IsValid)
            {
                ViewData["id"] = id;
                return this.View(model);
            }
            var user = this.Data.Users.Find(id);
            var photo = user.Photos.FirstOrDefault(p => p.Id == model.Id);
            if (photo == null)
            {
                return this.RedirectToAction("ViewAll", "Users");
            }

            if (model.PhotoFile != null)
            {
                var fileName = photo.PhotoLink
                .Split('/')
                .Last()
                .Split('?')
                .First();

                var taskDelete = Task.Run(() => DropBoxManager.Delete(fileName));
                taskDelete.Wait();

                var fileExtension = model.PhotoFile.FileName.Split('.').Last();
                var uniqueName = user.Id + Guid.NewGuid() + "." + fileExtension;

                var taskUpload = Task.Run(() => DropBoxManager.Upload(model.PhotoFile.InputStream, uniqueName));
                taskUpload.Wait();
                var fileMetadata = taskUpload.Result;

                photo.PhotoLink = fileMetadata.Url.Substring(0, fileMetadata.Url.IndexOf("?")) + "?raw=1"; ;
            }

            photo.Title = model.Title;
            this.Data.SaveChanges();

            return this.RedirectToAction("ViewAll", "Users", new { id = photo.Id });
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