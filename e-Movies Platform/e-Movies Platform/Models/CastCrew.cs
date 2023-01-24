using System.ComponentModel.DataAnnotations;

namespace e_Movies_Platform.Models
{
    public class CastCrew
    {
        public int? Id { get; set; }

        public CastCrewRole? Role { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }
        public ICollection<Movie>? Movies { get; set; }
    }
}
