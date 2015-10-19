namespace PhotoContest.App.Controllers
{
    #region

    using System.Web.Mvc;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.App.Models.BindingModels;
    using System.Threading.Tasks;
    using PhotoContest.App.Helpers;
    using System;
    using System.Linq;
    using AutoMapper;
    using PhotoContest.Models.Models;

    #endregion

    public class PhotosController : BaseController
    {

        public PhotosController(IPhotoContestData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.Redirect("/Photos/ViewPicture");
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

            byte[] buffer = null;
            using (var fs = model.PhotoFile.InputStream)
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }

            var task = Task.Run(() => DropBoxManager.Upload(uniqueName, buffer));
            task.Wait();

            var photo = Mapper.Map<AddPhotoBindingModel, Photo>(model);
            photo.PhotoFileName = uniqueName;
            this.Data.Photos.Add(photo);
            this.Data.SaveChanges();

            return this.RedirectToAction("ViewPicture", "Photos");
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