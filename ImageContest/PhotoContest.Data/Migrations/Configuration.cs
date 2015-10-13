namespace PhotoContest.Data.Migrations
{
    #region

    using System.Data.Entity.Migrations;

    #endregion

    public sealed class Configuration : DbMigrationsConfiguration<PhotoContestDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(PhotoContestDbContext context)
        {
        }
    }
}