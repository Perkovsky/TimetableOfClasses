using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TimetableOfClasses.Models.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string SelectedRole { get; set; }
        public SelectList Roles { get; set; }
        //TODO: в рамках тестового задания не делал подтверждающий пароль
        public string Password { get; set; }
    }
}
