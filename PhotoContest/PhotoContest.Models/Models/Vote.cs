namespace PhotoContest.Models.Models
{
    #region

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class Vote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1,5)]
        public int Stars { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        [ForeignKey("Photo")]
        public int PhotoId { get; set; }
        
        public virtual Photo Photo { get; set; }

        [ForeignKey("Contest")]
        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }
    }
}