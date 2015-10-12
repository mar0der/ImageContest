namespace ImageContest.Data
{
    #region

    using System.Data.Entity;

    using ImageContest.Data.Interfaces;
    using ImageContest.Data.Migrations;
    using ImageContest.Models.Models;

    using Microsoft.AspNet.Identity.EntityFramework;

    #endregion

    public class ImageContextDbContext : IdentityDbContext<User>, IImageContestDbContext
    {
        public ImageContextDbContext()
            : base("ImageContest", false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ImageContextDbContext, Configuration>());
        }

        public static ImageContextDbContext Create()
        {
            return new ImageContextDbContext();
        }
    }
}