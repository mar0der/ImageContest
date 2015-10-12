namespace ImageContest.App.Controllers
{
    #region

    using System.Threading;
    using System.Web.Mvc;

    using ImageContest.Data.Interfaces;
    using ImageContest.Models.Models;

    using Microsoft.AspNet.Identity;

    #endregion

    public class BaseController : Controller
    {
        public BaseController(IImageContestData data)
        {
            this.Data = data;
        }

        public IImageContestData Data { get; set; }

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