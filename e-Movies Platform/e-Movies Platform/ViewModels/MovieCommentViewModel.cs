using e_Movies_Platform.Models;

namespace e_Movies_Platform.ViewModels
{
    public class MovieCommentViewModel
    {
        public String Title { get; set; }
        public List<MovieComment> ListOfComments { get; set; }
        public string Comment { get; set; }
        public int MoviesId { get; set; }
        public int Rating { get; set; }
    }
}
