using PhotoContest.Models.Models;
using System.Collections.Generic;
using PhotoContest.Models.Enumerations;

namespace PhotoContest.App.Models.BindingModels.Contests
{
    public class ViewPhotoModel
    {
        public int Id { get; set; }

        public int ContestId { get; set; }

        public string Title { get; set; }

        public string ContestTitle { get; set; }

        public string PhotoLink { get; set; }

        public ICollection<Vote> Votes { get; set; }

        public User User { get; set; }
    }
}