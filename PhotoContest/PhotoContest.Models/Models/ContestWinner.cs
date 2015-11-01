using PhotoContest.Models.Enumerations;
using System.ComponentModel.DataAnnotations;
namespace PhotoContest.Models.Models
{
    public class ContestWinner
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PhotoId { get; set; }

        [Required]
        public int ContestId { get; set; }

        public Contest Contest { get; set; }

        [Required]
        public Places Place { get; set; }
    }
}
