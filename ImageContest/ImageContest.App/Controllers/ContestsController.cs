namespace ImageContest.App.Controllers
{
    #region

    using System.Web.Mvc;

    using ImageContest.Data.Interfaces;

    #endregion

    public class ContestsController : BaseController
    {
        public ContestsController(IImageContestData data)
            : base(data)
        {
        }

        // GET: Contests
        public ActionResult Index()
        {
            return this.View();
        }
    }
}