namespace PhotoContest.App.Areas.Admin.Controllers
{
    #region

    using System.Web.Mvc;

    using PhotoContest.App.Controllers;
    using PhotoContest.Data.Interfaces;

    #endregion

    [Authorize(Roles = "Admin")]
    public class BaseAdminController : BaseController
    {
        public BaseAdminController(IPhotoContestData data)
            : base(data)
        {
        }
    }
}