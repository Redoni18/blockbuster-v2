namespace e_Movies_Platform.Models
{
    public class MovieComment
    {
        public int Id { get; set; }
        public string Comments { get; set; }

        public DateTime PublishedDate { get; set; }

        public int MoviesId { get; set; }

        public Movie Movies { get; set; }

        public int Rating  { get; set; }   
    }
}
