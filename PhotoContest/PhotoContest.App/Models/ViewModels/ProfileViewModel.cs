namespace PhotoContest.App.Models.ViewModels
{
    using PhotoContest.Models.Models;
    using System.Collections.Generic;

    public class ProfileViewModel
    {
        public string Username { get; set; }

        public string Email { get; set; }
        
        public IEnumerable<Contest> OwnContests { get; set; }

        public IEnumerable<Contest> JudgeContests { get; set; }
    }
}