namespace PhotoContest.Models.Models
{
    #region

    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Enumerations;
    using System;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    #endregion

    public class User : IdentityUser
    {
        private ICollection<Vote> votes;

        private ICollection<Contest> contests;

        private ICollection<Contest> ownContests;

        private ICollection<Contest> judgeContests; 

        public User()
        {
            this.votes = new HashSet<Vote>();
            this.contests = new HashSet<Contest>();
            this.ownContests = new HashSet<Contest>();
            this.judgeContests = new HashSet<Contest>();
        }

        public Gender Gender { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual ICollection<Vote> Votes
        {
            get
            {
                return this.votes;
            }

            set
            {
                this.votes = value;
            }
        }

        public virtual ICollection<Contest> Contests
        {
            get
            {
                return this.contests;
            }

            set
            {
                this.contests = value;
            }
        }

        public virtual ICollection<Contest> OwnContests
        {
            get
            {
                return this.ownContests;
            }

            set
            {
                this.ownContests = value;
            }
        }

        public virtual ICollection<Contest> JudgeContests
        {
            get
            {
                return this.judgeContests;
            }

            set
            {
                this.judgeContests = value;
            }
        }




        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }
    }
}