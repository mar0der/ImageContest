namespace PhotoContest.App.Controllers
{
    #region

    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models.Models;

    #endregion


    [Authorize]
    public class BaseController : Controller
    {
        public BaseController(IPhotoContestData data)
        {
            this.Data = data;
        }

        public BaseController(IPhotoContestData data, PhotoContest.Models.Models.User user)
        {
            this.Data = data;
            this.CurrentUser = user;
        }

        public IPhotoContestData Data { get; private set; }

        public User CurrentUser { get; set; }

        protected override IAsyncResult BeginExecute(
            RequestContext requestContext, 
            AsyncCallback callback, 
            object state)
        {
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var username = requestContext.HttpContext.User.Identity.Name;
                var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);
                this.CurrentUser = user;
            }

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}