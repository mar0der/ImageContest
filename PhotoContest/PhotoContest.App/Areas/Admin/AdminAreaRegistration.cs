namespace PhotoContest.App.Areas.Admin
{
    #region

    using System.Web.Mvc;

    #endregion

    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_Default", 
                "Admin/{controller}/{action}/{id}", 
                new { action = "Index", id = UrlParameter.Optional }, 
                new[] { "PhotoContest.App.Areas.Admin.Controllers" });
        }
    }
}