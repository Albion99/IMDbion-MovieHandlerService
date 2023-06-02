using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMDbion_MovieHandlerService.Models
{
    [Table("movies")]
    public class Movie
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("title")]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("description")]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("genre")]
        public string Genre { get; set; }

        [Required]
        [Column("length")]
        public int Length { get; set; }

        [Required]
        [Column("publicationDate")]
        public DateTime PublicationDate { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("countryOfOrigin")]
        public string CountryOfOrigin { get; set; }

        [NotMapped]
        public List<Actor> Actors { get; set;}

        [Required]
        [Column("videoPath")]
        public string VideoPath { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }

        [Column("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
