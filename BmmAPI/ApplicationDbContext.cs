using BmmAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;

namespace BmmAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // modelBuilder.Entity<MoviesActors>()
            //    .HasKey(x => new { x.ActorId, x.MovieId });

            modelBuilder.Entity<MoviesGenres>()
                .HasKey(x => new { x.GenreId, x.MovieId });

            modelBuilder.Entity<MovieTheatersMovies>()
                .HasKey(x => new { x.MovieTheaterId, x.MovieId });


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Genre> Genres{ get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieTheater> MoviesTheaters { get; set;}

        public DbSet<Movie> Movies { get; set; }
        //public DbSet<MoviesActors>MoviesActors { get; set; }

        public DbSet<MoviesGenres> MoviesGenres { get; set; }
        public DbSet<MovieTheatersMovies> MoviesTheatersMovies { get; set; }




    }

}
