namespace PhotoContest.App.Models.ViewModels.Contests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using PhotoContest.Models.Enumerations;

    public class ContestViewModel
    {
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

        public int MaxNumberOfParticipants { get; set; }
    }
}