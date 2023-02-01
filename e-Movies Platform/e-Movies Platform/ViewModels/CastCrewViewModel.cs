using e_Movies_Platform.Models;

namespace e_Movies_Platform.ViewModels
{
    public class CastCrewViewModel
    {
        public int Id { get; set; }
        public List<CastCrewRole>? Roles { get; set; }  
        
        public CastCrewRole? Role { get; set; }
        public int? RoleId { get; set; }

        public string? FullName { get; set; }

    }
}
