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
            : base("PhotoContest", false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PhotoContestDbContext, Configuration>());
        }

        public virtual IDbSet<Contest> Contests { get; set; }

        public virtual IDbSet<Photo> Photos { get; set; }

        public virtual IDbSet<Vote> Votes { get; set; }

        public static PhotoContestDbContext Create()
        {
            return new PhotoContestDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Contest>()
                .HasMany(c => c.Participants)
                .WithMany(c => c.Contests)
                .Map(m =>
                {
                    m.MapLeftKey("ContestId");  // because it is the "left" column, isn't it?
                    m.MapRightKey("ParticipantId"); // because it is the "right" column, isn't it?
                    m.ToTable("ContestParticipants");
                });
            modelBuilder
                .Entity<Contest>()
                .HasMany(c => c.Judges)
                .WithMany(j => j.JudgeContests)
                .Map(m =>
                {
                    m.MapLeftKey("ContestId");  // because it is the "left" column, isn't it?
                    m.MapRightKey("JudgeId"); // because it is the "right" column, isn't it?
                    m.ToTable("ContestsJudges");
                });
            modelBuilder.Entity<Contest>()
                .HasRequired(c => c.Creator)
                .WithMany(cr => cr.OwnContests)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contest>()
                .HasMany(c => c.Votes)
                .WithRequired(v => v.Contest)
                .HasForeignKey(v => v.ContestId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}