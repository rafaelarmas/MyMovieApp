using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.FileIO;
using System.Reflection;

namespace MyMovieApp.Data
{
    public class MovieDataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public MovieDataContext(IConfiguration configuration)
        {
            Configuration = configuration;
            Database.OpenConnection();
            Database.EnsureCreated();

            if (Movies == null || Movies.Count() == 0)
                Seed();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("MyMovieDatabase"));
        }

        public override void Dispose()
        {
            Database.CloseConnection();
            base.Dispose();
        }

        public DbSet<Movie> Movies { get; set; }

        private void Seed()
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MyMovieApp.Data.Resources.mymoviedb.csv");

            if (stream == null)
                return;

            var parser = new TextFieldParser(stream)
            {
                Delimiters = [","],
                HasFieldsEnclosedInQuotes = true
            };
            var id = 1;

            while (!parser.EndOfData)
            {
                var parts = parser.ReadFields();
                if (parts == null || !DateTime.TryParse(parts[0], out DateTime releaseDate))
                    continue;

                var movie = new Movie();
                movie.MovieId = id;
                movie.ReleaseDate = releaseDate;
                
                if (parts.Length > 1)
                    movie.Title = parts[1];

                if (parts.Length > 2)
                    movie.Overview = parts[2];

                if (parts.Length > 3)
                    movie.Popularity = parts[3];

                if (parts.Length > 4 && decimal.TryParse(parts[4], out decimal voteCount))
                    movie.VoteCount = voteCount;

                if (parts.Length > 5 && decimal.TryParse(parts[5], out decimal voteAverage))
                    movie.VoteAverage = voteAverage;

                if (parts.Length > 6)
                    movie.OriginalLanguage = parts[6];

                if (parts.Length > 7)
                    movie.Genre = parts[7];

                if (parts.Length > 8)
                    movie.PosterUrl = parts[8];
                Movies.Add(movie);
                id++;
            }

            parser.Close();

            SaveChanges();
        }
    }
}
