using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimetableOfClasses.Models
{
    public class Discipline
    {
        public int DisciplineId { get; set; }
        [Required]
        public string Name { get; set; }
        public List<TeacherDiscipline> TeacherDisciplines { get; set; } = new List<TeacherDiscipline>();
    }
}
