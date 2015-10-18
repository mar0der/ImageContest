namespace PhotoContest.Data.Interfaces
{
    #region

    using PhotoContest.Models.Models;

    #endregion

    public interface IPhotoContestData
    {
        IRepository<User> Users { get; }

        IRepository<Contest> Contests { get; }

        IRepository<Photo> Photos { get; }

        int SaveChanges();
    }
}