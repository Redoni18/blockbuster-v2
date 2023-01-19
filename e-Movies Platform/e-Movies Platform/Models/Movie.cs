using Microsoft.Build.Framework;

namespace e_Movies_Platform.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? CoverImage { get; set; }
        [Required]
        public string? MovieLink { get; set; }
        [Required]
        public bool? IsPG { get; set; } = true;
        [Required]
        public int? Year { get; set; }
        [Required]
        public Genre? Genre { get; set; }
        public ICollection<CastCrew>? Cast { get; set; }
    }
}
