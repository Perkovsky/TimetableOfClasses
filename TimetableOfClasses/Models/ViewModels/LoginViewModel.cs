using System.ComponentModel.DataAnnotations;

namespace TimetableOfClasses.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [UIHint("password")]
        public string Password { get; set; }
    }
}
