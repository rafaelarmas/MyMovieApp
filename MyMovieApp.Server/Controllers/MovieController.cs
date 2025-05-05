using MyMovieApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyMovieApp.Server.Controllers
{
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private readonly MovieDataContext _movieDataContext;

        public MovieController(ILogger<MovieController> logger, MovieDataContext context)
        {
            _logger = logger;
            _movieDataContext = context;
        }


        [HttpGet]
        [Route("getmoviesbykeyword")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByKeyword(string? keyword, int pageSize, int currentPage, string? genre)
        {
            try
            {
                var query = _movieDataContext.Movies.AsQueryable();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(x => x.Title.ToUpper().Contains(keyword.ToUpper()));
                }
                
                if (!string.IsNullOrWhiteSpace(genre))
                {
                    query = query.Where(x => x.Genre.ToUpper().Contains(genre.ToUpper()));
                }

                var startIndex = currentPage > 1 ? pageSize * (currentPage - 1) : 0;

                return await query.Skip(startIndex).Take(pageSize).ToArrayAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get Movies failed.");
                return BadRequest(ex);
            }
            
        }

        [HttpGet]
        [Route("getgenres")]
        public async Task<ActionResult<IEnumerable<string>>> GetGenres()
        {
            try
            {
                var genres = new List<string>();
                var genreList = new List<string>();
                genreList.AddRange(await _movieDataContext.Movies.Select(x => x.Genre).Distinct().ToArrayAsync());
                foreach(var genreString in genreList)
                {
                    var genreParts = genreString.Split(',');
                    for(int i = 0; i < genreParts.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(genreParts[i]) && !genres.Contains(genreParts[i].Trim()))
                        {
                            genres.Add(genreParts[i].Trim());
                        }
                    }
                }
                return genres.Order().ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get Genres failed.");
                return BadRequest(ex);
            }

        }
    }
}
