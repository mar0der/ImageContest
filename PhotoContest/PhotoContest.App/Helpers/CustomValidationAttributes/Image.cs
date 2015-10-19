using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoContest.App.Helpers.CustomValidationAttributes
{
    public class Image : ValidationAttribute
    {
        public readonly IList<string> allowedTypes = new List<string>() { "image/png", "image/jpeg", "image/jpg"};

        public Image(long maxSizeInBytes)
        {
            this.MaxSizeInBytes = maxSizeInBytes;
        }

        public long MaxSizeInBytes { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var imageFile = value as HttpPostedFileWrapper;
            if (!allowedTypes.Contains(imageFile.ContentType))
            {
                return new ValidationResult("Please upload a photo."); ;
            }
            if (MaxSizeInBytes < imageFile.ContentLength)
            {
                var maxSizeInMB = this.MaxSizeInBytes / 1024 / 1024;
                return new ValidationResult(string.Format("Max allowed size is: {0:F2} MB", maxSizeInMB)); ;
            }

           return null;
        }
    }
}