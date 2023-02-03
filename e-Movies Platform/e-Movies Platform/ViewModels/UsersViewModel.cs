using System;
using System.ComponentModel.DataAnnotations;

namespace e_Movies_Platform.ViewModels
{
    public class UsersViewModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        public int Age
        {
            get
            {
                DateTime birth = Birthday;
                DateTime today = DateTime.Now;
                TimeSpan span = today - birth;
                DateTime age = DateTime.MinValue + span;

                return age.Year - 1;
            }
        }
    }
}
