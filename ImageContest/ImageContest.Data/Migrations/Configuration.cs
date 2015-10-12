namespace ImageContest.Data.Migrations
{
    #region

    using System.Data.Entity.Migrations;

    #endregion

    public sealed class Configuration : DbMigrationsConfiguration<ImageContestDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ImageContestDbContext context)
        {
        }
    }
}