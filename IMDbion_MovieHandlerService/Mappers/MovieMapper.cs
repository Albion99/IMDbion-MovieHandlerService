using AutoMapper;
using IMDbion_MovieHandlerService.DTO;
using IMDbion_MovieHandlerService.Models;

namespace IMDbion_MovieHandlerService.Mappers
{
    public class MovieMapper : Profile
    {
        public MovieMapper() 
        {
            CreateMap<Movie, MovieDTO>().ReverseMap();
        }
    }
}
