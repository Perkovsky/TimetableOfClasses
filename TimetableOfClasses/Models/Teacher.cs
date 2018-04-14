using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TimetableOfClasses.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Phone { get; set; }
        public List<TeacherDiscipline> TeacherDisciplines { get; set; } = new List<TeacherDiscipline>();

        public string GetDisciplines(IDisciplineRepository repository)
        {
            string result = "";
            var disciplines = TeacherDisciplines.Join(repository.Disciplines,
                    t => t.DisciplineId,
                    d => d.DisciplineId,
                    resultSelector: (t, d) => new { d.Name });
            foreach (var item in disciplines)
            {
                result += $"{item.Name}, ";
            }
            return string.IsNullOrEmpty(result) ? "" : result.Substring(0, result.Length - 2);
        }
    }
}
