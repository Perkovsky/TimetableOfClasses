using System.Collections.Generic;

namespace TimetableOfClasses.Models.ViewModels
{
    public class TeacherViewModel
    {
        public Teacher Teacher { get; set; }
        public IList<Discipline> AllDiscipline { get; set; }
        public IList<DisciplineCheckedViewModel> DisciplineChecked { get; set; }
    }
}
