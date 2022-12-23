using System.ComponentModel.DataAnnotations;

namespace e_Movies_Platform.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [Display(Name = "Genre Name")]
        public string? GenreName { get; set; }
    }
}
