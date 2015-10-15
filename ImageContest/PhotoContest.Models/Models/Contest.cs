namespace PhotoContest.Models.Models
{
    #region

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using PhotoContest.Models.Enumerations;

    #endregion

    public class Contest
    {
        // a title, description, reward strategy, voting strategy, participation strategy and deadline strategy. A contest also keeps track of all pictures submitted to it and their votes.
        private ICollection<User> participants;

        private ICollection<Photo> photos;

        private ICollection<Vote> votes;

        private ICollection<User> judges;

        public Contest()
        {
            this.participants = new HashSet<User>();
            this.photos = new HashSet<Photo>();
            this.votes = new HashSet<Vote>();
            this.judges = new HashSet<User>();
        }
        
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime Deadline { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public ContestStatus Status { get; set; }

        public RewardStrategy RewardStrategy { get; set; }

        public VotingStrategy VotingStrategy { get; set; }

        public ParticipationStrategy ParticipationStrategy { get; set; }

        public DeadlineStrategy DeadlineStrategy { get; set; }

        [ForeignKey("Creator")]
        public string CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public virtual ICollection<Photo> Photos
        {
            get
            {
                return this.photos;
            }

            set
            {
                this.photos = value;
            }
        }

        public virtual ICollection<User> Participants
        {
            get
            {
                return this.participants;
            }

            set
            {
                this.participants = value;
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

        public virtual ICollection<User> Judges
        {
            get
            {
                return this.judges;
            }

            set
            {
                this.judges = value;
            }
        }

    }
}