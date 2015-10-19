﻿namespace PhotoContest.App.Controllers
{
    #region

    using System.Linq;
    using System.Web.Mvc;
    using PhotoContest.Data.Interfaces;
    using System.Web;
    using PhotoContest.App.Models.ViewModels;
    using PhotoContest.Models.Models;
    using AutoMapper;
    using PhotoContest.App.Models.BindingModels.Users;

    #endregion

    public class UsersController : BaseController
    {
        public UsersController(IPhotoContestData data)
            : base(data)
        {
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("profile", "users", new { username = this.User.Identity.Name });
            }

            return this.HttpNotFound();
        }

        [Route("users/profile/{username}")]
        [AllowAnonymous]
        public ActionResult Profile(string username)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return this.HttpNotFound();
            }

            var viewModel = Mapper.Map<User,ProfileViewModel>(user);

            return this.View(viewModel);
        }

        [HttpGet]
        public ActionResult Edit()
        {
            var model = Mapper.Map<User, EditProfileModel>(this.CurrentUser);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditProfileModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                this.CurrentUser.PhoneNumber = model.PhoneNumber;
                this.CurrentUser.Email = model.Email;
                this.CurrentUser.Gender = model.Gender;
                this.CurrentUser.DateOfBirth = model.DateOfBirth;
                this.CurrentUser.Address = model.Address;
                this.Data.SaveChanges();
                return RedirectToAction("profile", "users", new { username = this.User.Identity.Name });
            }

            return this.View(model);
        }
    }
}