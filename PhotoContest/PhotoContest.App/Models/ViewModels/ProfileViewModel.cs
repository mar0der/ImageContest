namespace PhotoContest.App.Models.ViewModels
{
    using PhotoContest.Models.Enumerations;
    using PhotoContest.Models.Models;
    using System;
    using System.Collections.Generic;

    public class ProfileViewModel
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }

        public IEnumerable<Contest> OwnContests { get; set; }

        public IEnumerable<Contest> JudgeContests { get; set; }
    }
}