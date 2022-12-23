using e_Movies_Platform.Models;

namespace e_Movies_Platform.ViewModels
{
    public class MovieViewModel

    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? CoverImage { get; set; }
        public string? MovieLink { get; set; }
        public bool? IsPG { get; set; } = true;
        public int? Year { get; set; }
        public List<Genre>? Genres { get; set; }
        public int? GenreId { get; set; }
    }
}

