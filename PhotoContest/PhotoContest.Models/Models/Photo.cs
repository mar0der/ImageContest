namespace PhotoContest.Models.Models
{
    #region

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    #endregion

    public class Photo
    {
        private ICollection<Contest> contests;

        private ICollection<Vote> votes;

        public Photo()
        {
            this.votes = new HashSet<Vote>();
            this.contests = new HashSet<Contest>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string PhotoLink { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public bool IsProfile { get; set; }

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
    }
}