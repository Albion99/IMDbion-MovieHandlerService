using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMDbion_MovieHandlerService.Models
{
    [Table("movies_actors")]
    public class MovieActor
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Column("movieId")]
        public Guid MovieId { get; set; }
        [Required]
        [Column("actorId")]
        public Guid ActorId { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }

        [Column("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
