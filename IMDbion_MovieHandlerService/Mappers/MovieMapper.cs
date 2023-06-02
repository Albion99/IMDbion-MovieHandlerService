using AutoMapper;
using IMDbion_MovieHandlerService.DTOs;
using IMDbion_MovieHandlerService.Models;

namespace IMDbion_MovieHandlerService.Mappers
{
    public class MovieMapper : Profile
    {
        public MovieMapper() 
        {
            CreateMap<Movie, MovieDTO>().ReverseMap();
            CreateMap<MovieDTO, MovieCreateDTO>().ReverseMap();
            CreateMap<Movie, MovieUpdateDTO>().ReverseMap();
            CreateMap<Movie, MovieCreateDTO>().ReverseMap();
        }
    }
}
