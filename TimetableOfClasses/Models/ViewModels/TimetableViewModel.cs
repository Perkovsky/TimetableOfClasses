using Microsoft.AspNetCore.Mvc.Rendering;

namespace TimetableOfClasses.Models.ViewModels
{
    public class TimetableViewModel
    {
        public Timetable Timetable { get; set; }
        public SelectList Disciplines { get; set; }
        public SelectList Teachers { get; set; }
        public SelectList Groups { get; set; }
        public string SelectedDiscipline { get; set; }
        public string SelectedTeacher { get; set; }
        public string SelectedGroup { get; set; }
    }
}
