namespace PhotoContest.App.Models.BindingModels.Contests
{
    public class VoteBindingModel
    {
        public int Stars { get; set; }

        public int PhotoId { get; set; }

        public int ContestId { get; set; }
    }
}