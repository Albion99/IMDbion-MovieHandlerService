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
        [Column("title", TypeName = "varchar(100)")]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("description", TypeName = "text")]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("genre", TypeName = "varchar(100)")]
        public string Genre { get; set; }

        [Required]
        [Column("length", TypeName = "int(11)")]
        public int Length { get; set; }

        [Required]
        [Column("publicationDate")]
        public DateTime PublicationDate { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("countryOfOrigin", TypeName = "varchar(100)")]
        public string CountryOfOrigin { get; set; }

        [NotMapped]
        List<MovieActor> Actors { get; set;}

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }

        [Column("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
