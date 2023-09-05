using AutoMapper;
using BmmAPI.DTOs;
using BmmAPI.Entities;
using Microsoft.AspNetCore.Identity;
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

            CreateMap<MovieTheaterCreationDTO,MovieTheater>()
                .ForMember(x => x.Location, x => x.MapFrom(dto =>
                geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));


            CreateMap<MovieCreationDTO, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.MoviesGenres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(x => x.MovieTheatersMovies, options => options.MapFrom(MapMovieTheatersMovies));
            //.ForMember(x => x.MoviesActors, options => options.MapFrom(MapMoviesActors)); 


            CreateMap<Movie, MovieDTO>()
                .ForMember(x => x.Genres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(x => x.MovieTheaters, options => options.MapFrom(MapMovieTheatersMovies));

            CreateMap<IdentityUser, UserDTO>();

        }

        private List<MovieTheaterDTO> MapMovieTheatersMovies(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<MovieTheaterDTO>();

            if(movie.MovieTheatersMovies != null)
            {
                foreach (var movieTheatersMovies in movie.MovieTheatersMovies)
                {
                    result.Add(new MovieTheaterDTO()
                    {
                        Id = movieTheatersMovies.MovieTheaterId,
                        Name = movieTheatersMovies.MovieTheater.Name,
                        Latitude = movieTheatersMovies.MovieTheater.Location.Y,
                        Longitude = movieTheatersMovies.MovieTheater.Location.X

                    });

                }
                
            }

            return result;
        }

        private List<GenreDTO> MapMoviesGenres(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<GenreDTO>();

            if(movie.MoviesGenres != null)
            {
                foreach (var genre in movie.MoviesGenres)
                {
                    result.Add(new GenreDTO() { Id = genre.GenreId, Name = genre.Genre.Name });
                }
            }

            return result;
        }

        private List<MoviesGenres>MapMoviesGenres(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesGenres>();

            if (movieCreationDTO.GenresIds == null) { return result; }
            
            foreach(var id in movieCreationDTO.GenresIds)
            {
                result.Add(new MoviesGenres() { GenreId=id });
            }
            return result;
        }    

        private List<MovieTheatersMovies> MapMovieTheatersMovies(MovieCreationDTO movieCreationDTO,
            Movie movie)
        {
            var result = new List<MovieTheatersMovies>();

            if(movieCreationDTO.MovieTheatersIds == null) { return result;}

            foreach(var id in movieCreationDTO.MovieTheatersIds)
            {
                result.Add(new MovieTheatersMovies() { MovieTheaterId = id });

            }
            return result;
        }

       /*private List<MoviesActors> MapMoviesActors(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesActors>();

            if (movieCreationDTO.Actors == null) { return result; }

            foreach(var actor in movieCreationDTO.Actors)
            {
                result.Add(new MoviesActors() { ActorId = actor.Id, Character = actor.Character });
            }
            return result;
        }
       */
       
    }
}
