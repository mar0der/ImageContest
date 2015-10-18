namespace PhotoContest.Data.Repositories
{
    #region

    using System;
    using System.Collections.Generic;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models.Models;

    #endregion

    public class PhotoContestData : IPhotoContestData
    {
        private readonly IPhotoContestDbContext context;

        private readonly IDictionary<Type, object> repositories;

        public PhotoContestData(IPhotoContestDbContext context)
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

        public IRepository<Contest> Contests
        {
            get
            {
                return this.GetRepository<Contest>();
            }
        }

        public IRepository<Photo> Photos
        {
            get
            {
                return this.GetRepository<Photo>();
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