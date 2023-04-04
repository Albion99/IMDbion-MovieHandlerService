using System.ComponentModel.DataAnnotations.Schema;

namespace IMDbion_MovieHandlerService.Models
{
    [Table("movies")]
    public class Movie
    {
        public Guid Id;
        public string Title;
        public string Description;
        public string Summary;
        public string Genre;
        public string Length;
        public DateTime PublicationDate;
        public string CountryOfOrigin;
        public DateTime CreatedAt;
        public DateTime UpdatedAt;

        public Movie(Guid id, string title, string description, string summary, string genre, string length, DateTime publicationDate, string countryOfOrigin, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            Title = title;
            Description = description;
            Summary = summary;
            Genre = genre;
            Length = length;
            PublicationDate = publicationDate;
            CountryOfOrigin = countryOfOrigin;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
