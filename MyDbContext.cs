using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MoviesDB
{

    class SqliteDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Hall_Movie> HallsMovies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString: "FileName=HallsMoviesDb1", option =>
            {
                option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Movie>().HasMany(c => c.Halls).WithMany(s => s.Movies).UsingEntity<Hall_Movie>
                (j => j.HasOne(pt => pt.Hall).WithMany(t => t.Hall_Movies).HasForeignKey(pt => pt.HallId),
                j => j.HasOne(pt => pt.Movie).WithMany(p => p.Hall_Movies).HasForeignKey(pt => pt.MovieId),
                j =>
                {
                    j.HasKey(t => new { t.MovieId, t.HallId });
                    j.ToTable("Hall_Movie");
                });
        }
    }

    class Movie
    {
        public int MovieId { get; set; }
        [Required]
        public string Movie_Name { get; set; }
        public DateTime start_datetime { get; set; }
        public List<Hall> Halls { get; set; } = new List<Hall>();
        public List<Hall_Movie> Hall_Movies { get; set; } = new List<Hall_Movie>();
    }

    class Hall
    {
        public int HallId { get; set; }
        [Required]
        public string Hall_Name { get; set; }
        public int Place_count { get; set; }
        public List<Movie> Movies { get; set; } = new List<Movie>();
        public List<Hall_Movie> Hall_Movies { get; set; } = new List<Hall_Movie>();
    }

    class Hall_Movie
    {
        public int HallId { get; set; }
        public Hall Hall { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
