using Microsoft.Extensions.Configuration;
using MyMovieApp.Data;

namespace MyMovieApp.Tests
{
    public class DataTests
    {
        private MovieDataContext _context;

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(
                [
                    new("ConnectionStrings:MyMovieDatabase", "Data Source=test.db")
                ])
                .Build();
            _context = new MovieDataContext(config);
        }

        [TearDown]
        public void TearDown()
        {
            if (_context.Movies.Any(x => x.MovieId == 1))
            {
                _context.Movies.Remove(_context.Movies.Single(x => x.MovieId == 1));
                _context.SaveChanges();
            }

            _context.Dispose();
        }

        [Test]
        public void AddMovieToDatabaseTest()
        {
            var movie = new Movie
            {
                MovieId = 1,
                Title = "Test1",
                ReleaseDate = DateTime.Now,
                Genre = "TestGenre"
            };

            _context.Movies.Add(movie);
            _context.SaveChanges();

            var output = _context.Movies.Single(x => x.MovieId == 1);

            Assert.That(output.Title, Is.EqualTo(movie.Title));
            Assert.That(output.Genre, Is.EqualTo(movie.Genre));
        }
    }
}
