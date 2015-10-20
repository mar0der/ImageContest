namespace PhotoContest.App.Controllers
{
    #region

    using System.Web.Mvc;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.App.Models.Photos;
    using System.Threading.Tasks;
    using PhotoContest.App.Helpers;
    using EntityFramework.Extensions;
    using System.Linq;
    using AutoMapper;
    using PhotoContest.Models.Models;
    using PhotoContest.App.Models.ViewModels;
    using PhotoContest.App.Models.Photos.BindingModels;
    using System.Linq.Expressions;
    using System;

    #endregion

    public class PhotosController : BaseController
    {

        public PhotosController(IPhotoContestData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.Redirect("/Photos/All");
        }

        public ActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Add(AddPhotoBindingModel model)
        {
            if (!this.ModelState.IsValid || model == null)
            {
                return this.View();
            }

            var fileExtension = model.PhotoFile.FileName.Split('.').Last();
            var uniqueName = this.CurrentUser.Id + Guid.NewGuid() + "." + fileExtension;

            var task = Task.Run(() => DropBoxManager.Upload(model.PhotoFile.InputStream, uniqueName));
            task.Wait();
            var photoLink = task.Result;

            var photo = Mapper.Map<AddPhotoBindingModel, Photo>(model);
            photo.PhotoLink = photoLink.Url.Substring(0, photoLink.Url.IndexOf("?")) + "?raw=1";
            this.CurrentUser.Photos.Add(photo);
            this.Data.SaveChanges();

            return this.RedirectToAction("ViewPicture", "Photos", new { id = photo.Id });
        }

        //change this to custom route /view and custom view template. because view is reserved word
        public ActionResult ViewPicture(int id)
        {
            var photo = this.Data.Photos.Find(id);
            if (photo == null)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var photoModel = Mapper.Map<Photo, PhotoViewModel>(photo);
            photoModel.PhotoLink = photo.PhotoLink;

            return this.View(photoModel);
        }

        [HttpGet]
        [Route("photos/edit/{id}")]
        public ActionResult Edit(int id)
        {
            var photo = this.CurrentUser.Photos.FirstOrDefault(p => p.Id == id);
            if (photo == null)
            {
                return this.RedirectToAction("All", "Photos");
            }

            var photoViewModel = Mapper.Map<Photo, EditPhotoModel>(photo);

            return this.View(photoViewModel);
        }

        [HttpPost]
        [Route("photos/edit/{id}")]
        public ActionResult Edit(EditPhotoModel model)
        {
            if (model == null || !this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var photo = this.CurrentUser.Photos.FirstOrDefault(p => p.Id == model.Id);
            if (photo == null)
            {
                return this.RedirectToAction("All", "Photos");
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
                var uniqueName = this.CurrentUser.Id + Guid.NewGuid() + "." + fileExtension;

                var taskUpload = Task.Run(() => DropBoxManager.Upload(model.PhotoFile.InputStream, uniqueName));
                taskUpload.Wait();
                var fileMetadata = taskUpload.Result;

                photo.PhotoLink = fileMetadata.Url.Substring(0, fileMetadata.Url.IndexOf("?")) + "?raw=1"; ;
            }

            photo.Title = model.Title;
            this.Data.SaveChanges();

            return this.RedirectToAction("ViewPicture", "Photos", new { id = photo.Id });

        }

        public ActionResult All()
        {
            var photos = this.CurrentUser.Photos;

            return this.View(photos);
        }

        [Route("photos/delete/{id}")]
        public ActionResult Delete(int id)
        {
            var photo = this.CurrentUser.Photos.FirstOrDefault(p => p.Id == id);

            if (photo == null)
            {
                return this.RedirectToAction("All", "Photos");
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

            return this.RedirectToAction("All", "Photos");
        }

        [Route("photos/setAsProfile/{photoId}")]
        public ActionResult SetAsProfile(int photoId)
        {
            var photo = this.CurrentUser.Photos.FirstOrDefault(p => p.Id == photoId);
            if (photo == null)
            {
                return this.RedirectToAction("All", "Photos");
            }

            this.Data.Photos.All().Update(p => p.UserId == this.CurrentUser.Id, p => new Photo() { IsProfile = false });
            photo.IsProfile = true;
            this.Data.SaveChanges();

            return this.RedirectToAction("Profile", "Users", new { username = this.CurrentUser.UserName });
        }
    }
}