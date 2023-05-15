using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IMDbion_MovieHandlerService.DTOs
{
    public class MovieDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public int Length { get; set; }
        public DateTime PublicationDate { get; set; }
        public string CountryOfOrigin { get; set; }
        public List<Guid> ActorIds { get; set; }    
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
