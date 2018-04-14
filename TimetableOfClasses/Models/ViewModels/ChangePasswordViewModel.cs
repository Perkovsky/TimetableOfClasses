using System.ComponentModel.DataAnnotations;

namespace TimetableOfClasses.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        [Required]
        //TODO: в рамках тестового задания не делал подтверждающий пароль
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
