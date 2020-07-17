using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoManager.Models.Users
{
    public class EditVM
    {
        public int Id { get; set; }

        [DisplayName("Username: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public string Username { get; set; }
        [DisplayName("Email: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public string Email { get; set; }
        [DisplayName("Is Admin?")]
        [Required(ErrorMessage = "This field is Required!")]
        public bool IsAdmin { get; set; }
    }
}
