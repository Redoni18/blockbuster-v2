using e_Movies_Platform.Models;

namespace e_Movies_Platform.ViewModels
{
    public class MovieViewModel

    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? CoverImage { get; set; }
        public string? MovieLink { get; set; }
        public bool IsPG { get; set; } = false;
        public string? IsPGCheckbox { get; set; }
        public int? Year { get; set; }
        public Genre Genre { get; set; }
        public List<Genre>? Genres { get; set; }
        public int? GenreId { get; set; }
        public List<CastCrew>? Cast { get; set; }
        public List<CastCrew>? SelectedCast { get; set; }
        public List<int>? CastId { get; set; }
        public List<CastCrewRole>? Roles { get; set; }
        public int? RoleId { get; set; }
        public CastCrew? Director { get; set; }
        public int? DirectorId { get; set; }
    }
}

