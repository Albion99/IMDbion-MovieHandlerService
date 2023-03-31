namespace IMDbion_MovieHandlerService.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Genre { get; set; }
        public string Length { get; set; }
        public DateTime PublicationDate { get; set; }
        public string CountryOfOrigin { get; set; }

        public Movie(int id, string title, string description, string summary, string genre, string length, DateTime publicationDate, string countryOfOrigin)
        {
            Id = id;
            Title = title;
            Description = description;
            Summary = summary;
            Genre = genre;
            Length = length;
            PublicationDate = publicationDate;
            CountryOfOrigin = countryOfOrigin;
        }
    }
}
