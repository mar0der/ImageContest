using PhotoContest.Models.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PhotoContest.Models.Models
{
    public class ContestWinner
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PhotoId { get; set; }

        [Required]
        [ForeignKey("Contest")]
        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }

        [Required]
        public Places Place { get; set; }
    }
}
