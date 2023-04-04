using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMDbion_MovieHandlerService.Models
{
    [Table("movies")]
    public class Movie
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public string Length { get; set; }
        public DateTime PublicationDate { get; set; }
        public string CountryOfOrigin { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Movie() { }

        public Movie(Guid id, string title, string description, string genre, string length, DateTime publicationDate, string countryOfOrigin, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            Title = title;
            Description = description;
            Genre = genre;
            Length = length;
            PublicationDate = publicationDate;
            CountryOfOrigin = countryOfOrigin;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
