namespace ImageContest.Data.Migrations
{
    #region

    using System.Data.Entity.Migrations;

    #endregion

    public sealed class Configuration : DbMigrationsConfiguration<ImageContextDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ImageContextDbContext context)
        {
        }
    }
}