namespace PhotoContest.Data.Interfaces
{
    using PhotoContest.Models.Models;

    public interface IPhotoContestData
    {
        IRepository<User> Users { get; }

        int SaveChanges();
    }
}