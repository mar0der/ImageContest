using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoContest.App.Models.ViewModels
{
    public class PhotoViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string UserId { get; set; }

        public string PhotoLink { get; set; }
    }
}