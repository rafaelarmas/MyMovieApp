using Microsoft.Extensions.Configuration;
using MyMovieApp.Data;
using MyMovieApp.Server;
using MyMovieApp.Server.Controllers;

namespace MyMovieApp.Tests
{
    public class MovieControllerTests
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
            _context.Dispose();
        }

        [Test]
        public void FirstTenMoviesAreRetrievedTest()
        {
            var controller = new MovieController(null, _context);

            var movies = controller.GetMoviesByKeyword(null, 10, 1, null);

            Assert.That(movies.IsCompleted, Is.True);
            Assert.That(movies.Result.Value.Count, Is.EqualTo(10));
        }

        [Test]
        public void FirstTwentyFiveMoviesAreRetrievedTest()
        {
            var controller = new MovieController(null, _context);

            var movies = controller.GetMoviesByKeyword(null, 25, 1, null);

            Assert.That(movies.IsCompleted, Is.True);
            Assert.That(movies.Result.Value.Count, Is.EqualTo(25));
        }

        [Test]
        public void KeywordFilterTest()
        {
            var keyword = "batman";
            var controller = new MovieController(null, _context);

            var movies = controller.GetMoviesByKeyword(keyword, 10, 1, null);

            Assert.That(movies.IsCompleted, Is.True);
            Assert.That(movies.Result.Value.All(x => x.Title.ToLower().Contains(keyword)));
        }

        // TODO: Add more tests
    }
}
