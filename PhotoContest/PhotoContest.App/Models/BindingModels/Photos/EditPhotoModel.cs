using PhotoContest.App.Helpers.CustomValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoContest.App.Models.Photos.BindingModels
{
    public class EditPhotoModel
    {
        [Required]
        [Display(Name = "Title")]
        [StringLength(100, ErrorMessage = "Title must be at least 2 characters.", MinimumLength = 2)]

        public string Title { get; set; }
        [Image(2097152)]
        public HttpPostedFileBase PhotoFile { get; set; }

        public int Id { get; set; }

        public string PhotoLink { get; set; }
    }
}