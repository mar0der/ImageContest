using AutoMapper;

namespace PhotoContest.App.Areas.Admin.Controllers
{
    #region
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.App.Models.Photos.Users;
    using PhotoContest.Models.Models;

    #endregion

    public class UsersController : BaseAdminController
    {
        public UsersController(IPhotoContestData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.Redirect("/Admin/Users/ViewAll");
        }

        public ActionResult ViewAll()
        {
            var users = this.Data.Users.All().OrderBy(u => u.Id);
            return this.View(users);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            ViewData["id"] = id;
            var user = this.Data.Users.Find(id);
            var model = Mapper.Map<User, EditProfileModel>(user);
            //var profilePhoto = user.Photos.FirstOrDefault(p => p.IsProfile == true);
            //var defaultAvatar = "http://showdown.gg/wp-content/uploads/2014/05/default-user.png";
            //model.PhotoLink = profilePhoto != null ? profilePhoto.PhotoLink : defaultAvatar;

            return this.View(model);
        }

        [HttpPost]
        public ActionResult EditUser(string id, EditProfileModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var user = this.Data.Users.Find(id);
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;
                user.Gender = model.Gender;
                user.DateOfBirth = model.DateOfBirth;
                user.Address = model.Address;
                this.Data.SaveChanges();
                return RedirectToAction("ViewAll", "Users", new { username = this.User.Identity.Name });
            }

            return RedirectToAction("Edit","Users", new { id = id});
        }
    }
}