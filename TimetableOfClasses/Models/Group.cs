using System;
using System.ComponentModel.DataAnnotations;

namespace TimetableOfClasses.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        //TODO: в рамках тестового задания не делал контроль на даты
        // т.е. дата начала дожна быть меньше даты окончания и т.д.
        public DateTime DateBeginAction { get; set; }
        [Required]
        public DateTime DateEndAction { get; set; }
    }
}
