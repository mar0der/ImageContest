namespace ImageContest.Data.Repositories
{
    #region

    using System;
    using System.Collections.Generic;

    using ImageContest.Data.Interfaces;
    using ImageContest.Models.Models;

    #endregion

    public class ImageContestData : IImageContestData
    {
        private readonly IImageContestDbContext context;

        private readonly IDictionary<Type, object> repositories;

        public ImageContestData(IImageContestDbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<User> Users
        {
            get
            {
                return this.GetRepository<User>();
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfRepository = typeof(T);
            if (!this.repositories.ContainsKey(typeOfRepository))
            {
                var newRepository = Activator.CreateInstance(typeof(EfRepository<T>), this.context);
                this.repositories.Add(typeOfRepository, newRepository);
            }

            return (IRepository<T>)this.repositories[typeOfRepository];
        }
    }
}