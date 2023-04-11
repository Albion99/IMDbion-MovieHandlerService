namespace IMDbion_MovieHandlerService.DTOs
{
    public class MovieCreateDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public int Length { get; set; }
        public DateTime PublicationDate { get; set; }
        public string CountryOfOrigin { get; set; }
    }
}
