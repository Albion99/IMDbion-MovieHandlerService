using System.ComponentModel.DataAnnotations;

namespace IMDbion_MovieHandlerService.DTOs
{
    public class MovieCreateDTO
    {
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Only alphabetic characters are allowed.")]
        public string Title { get; set; }

        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Only alphabetic characters are allowed.")]
        public string Description { get; set; }

        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Only alphabetic characters are allowed.")]
        public string Genre { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Only numeric characters are allowed.")]
        public int Length { get; set; }

        public DateTime PublicationDate { get; set; }

        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Only alphabetic characters are allowed.")]
        public string CountryOfOrigin { get; set; }

        public List<Guid> ActorIds { get; set; }

        [RegularExpression(@"^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}\.mp4$", ErrorMessage = "Invalid video path format.")]
        public string VideoPath { get; set; }
    }
}
