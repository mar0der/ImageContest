namespace PhotoContest.App.Models.Photos.BindingModels
{
    using PhotoContest.App.Helpers.CustomValidationAttributes;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class AddPhotoBindingModel
    {
        [Required]
        [Display(Name = "Title")]
        [StringLength(100, ErrorMessage = "Title must be at least 2 characters.", MinimumLength = 2)]

        public string Title { get; set; }
        [Required]
        [Image(2097152)]
        public HttpPostedFileBase PhotoFile { get; set; }
    }
}