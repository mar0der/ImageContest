namespace ImageContest.App.Controllers
{
    #region

    using System.Threading;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models.Models;

    #endregion

    public class BaseController : Controller
    {
        public BaseController(IPhotoContestData data)
        {
            this.Data = data;
        }

        public IPhotoContestData Data { get; set; }

        public User CurrentUser
        {
            get
            {
                return this.Data.Users.Find(Thread.CurrentPrincipal.Identity.GetUserId());
            }
        }

        public string CurrentUserUsername
        {
            get
            {
                return Thread.CurrentPrincipal.Identity.GetUserName();
            }
        }

        public string CurrentUserId
        {
            get
            {
                return Thread.CurrentPrincipal.Identity.GetUserId();
            }
        }
    }
}