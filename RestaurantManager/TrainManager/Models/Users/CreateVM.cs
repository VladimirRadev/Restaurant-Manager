using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoManager.Models.Users
{
    public class CreateVM
    {
        [DisplayName("Username: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public string Username { get; set; }

        [DisplayName("Password: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public string Password { get; set; }

        [DisplayName("Confirm Password: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public string ConfirmPassword { get; set; }

        [DisplayName("Email: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public string Email { get; set; }
    }
}
