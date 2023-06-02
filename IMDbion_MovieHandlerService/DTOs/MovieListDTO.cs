namespace IMDbion_MovieHandlerService.DTOs
{
    public class MovieListDTO
    {
        public List<MovieDTO> Movies { get; set; }
        public int TotalPages { get; set; }
    }
}
