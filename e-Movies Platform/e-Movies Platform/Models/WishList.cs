using System.ComponentModel.DataAnnotations.Schema;

namespace e_Movies_Platform.Models
{
    public class WishList
    {
        public int? Id { get; set; }
        public string? Name { get; set; }    

        public string? Year { get; set; }

        public string? Genre { get; set; }

        public bool? Isapproved { get; set; } = false;
        public ApplicationUser? User { get; set; }
    }
}
