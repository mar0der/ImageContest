namespace PhotoContest.Data
{
    #region

    using System.Data.Entity;

    using Microsoft.AspNet.Identity.EntityFramework;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Data.Migrations;
    using PhotoContest.Models.Models;

    #endregion

    public class PhotoContestDbContext : IdentityDbContext<User>, IPhotoContestDbContext
    {
        public PhotoContestDbContext()
            : base("ImageContest", false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PhotoContestDbContext, Configuration>());
        }

        public virtual IDbSet<Contest> Contests { get; set; }

        public static PhotoContestDbContext Create()
        {
            return new PhotoContestDbContext();
        }
    }
}