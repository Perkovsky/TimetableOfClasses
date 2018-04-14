using System;
using System.Collections.Generic;

namespace TimetableOfClasses.Models.ViewModels
{
    public class TimetableOfClassesViewModel
    {
        public Group Group { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Timetable> Timetables { get; set; }
    }
}
