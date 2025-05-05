namespace MyMovieApp.Data
{
    /// <summary>
    /// Movie details are as follows:
    /// Release_Date,Title,Overview,Popularity,Vote_Count,Vote_Average,Original_Language,Genre,Poster_Url
    /// Example:
    /// 2021-12-15,Spider-Man: No Way Home,"Peter Parker is unmasked and no longer able to separate his normal life from the high-stakes of being a super-hero. 
    /// When he asks for help from Doctor Strange the stakes become even more dangerous, forcing him to discover what it truly means to be Spider-Man.",
    /// 5083.954,8940,8.3,en,"Action, Adventure, Science Fiction",https://image.tmdb.org/t/p/original/1g0dhYtq4irTY1GPXvft6k4YLjm.jpg
    /// </summary>
    public class Movie
    {
        public Movie()
        {
            Title = string.Empty;
            Overview = string.Empty;
            Popularity = string.Empty;
            VoteCount = 0;
            VoteAverage = 0;
            OriginalLanguage = string.Empty;
            Genre = string.Empty;
            PosterUrl = string.Empty;
        }

        public DateTime ReleaseDate { get; set; }
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string? Overview { get; set; }
        public string? Popularity { get; set; }
        public decimal? VoteCount { get; set; }
        public decimal? VoteAverage { get; set; }
        public string? OriginalLanguage { get; set; }
        public string Genre { get; set; }
        public string? PosterUrl { get; set; }
    }
}
