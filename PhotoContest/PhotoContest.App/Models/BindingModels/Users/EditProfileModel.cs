using System;
using System.Collections.Generic;
using PhotoContest.Models.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoContest.App.Models.BindingModels.Users
{
    public class EditProfileModel
    {
        [EmailAddress]
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public Gender Gender { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
    }
}