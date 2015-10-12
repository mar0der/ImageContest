namespace ImageContest.Data
{
    #region

    using System.Data.Entity;

    using ImageContest.Data.Interfaces;
    using ImageContest.Data.Migrations;
    using ImageContest.Models.Models;

    using Microsoft.AspNet.Identity.EntityFramework;

    #endregion

    public class ImageContestDbContext : IdentityDbContext<User>, IImageContestDbContext
    {
        public ImageContestDbContext()
            : base("ImageContest", false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ImageContestDbContext, Configuration>());
        }

        public static ImageContestDbContext Create()
        {
            return new ImageContestDbContext();
        }
    }
}