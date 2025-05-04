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
        public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByKeyword(string? keyword, int pageSize, int currentPage)
        {
            try
            {
                var query = _movieDataContext.Movies.AsQueryable();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(x => x.Title.ToUpper().Contains(keyword.ToUpper()));
                }
                //var count = await query.CountAsync();
                var startIndex = currentPage > 1 ? pageSize * (currentPage - 1) : 0;

                return await query.Skip(startIndex).Take(pageSize).ToArrayAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get Movies failed.");
                return BadRequest(ex);
            }
            
        }
    }
}
