using AutoMapper;
using BmmAPI.DTOs;
using BmmAPI.Entities;
using NetTopologySuite.Geometries;

namespace BmmAPI.Helpres
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory) 
        {
            CreateMap<GenreDTO,Genre>().ReverseMap();
            CreateMap<GenreCreationDTO, Genre>();
            CreateMap<ActorDTO, Actor>().ReverseMap();
            CreateMap<ActorCreationDTO, Actor>();
            CreateMap<MovieTheater, MovieTheaterDTO>()
               .ForMember(x => x.Latitude, dto => dto.MapFrom(prop => prop.Location.Y))
               .ForMember(x => x.Longitude, dto => dto.MapFrom(prop => prop.Location.X));

            CreateMap<MovieTheaterCreationDTO,MovieTheater>().ForMember(x => x.Location, x => x.MapFrom(dto =>
                geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));

            /*.ForMember(x => x.Picture, options => options.Ignore());*/
        }
    }
}
