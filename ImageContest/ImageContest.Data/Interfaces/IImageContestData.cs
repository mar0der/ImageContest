namespace ImageContest.Data.Interfaces
{
    using ImageContest.Models.Models;

    public interface IImageContestData
    {
        IRepository<User> Users { get; }

        int SaveChanges();
    }
}