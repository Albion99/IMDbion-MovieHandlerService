using IMDbion_MovieHandlerService.Models;

namespace IMDbion_MovieHandlerService.DTOs
{
    public class MovieUpdateDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public int Length { get; set; }
        public DateTime PublicationDate { get; set; }
        public List<Guid> ActorIds { get; set; }
        public string CountryOfOrigin { get; set; }
        public string VideoPath { get; set; }
    }
}
