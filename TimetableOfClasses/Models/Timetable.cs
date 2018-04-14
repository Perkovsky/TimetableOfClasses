using System;

namespace TimetableOfClasses.Models
{
    public class Timetable
    {
        public int TimetableId { get; set; }
        public Discipline Discipline { get; set; }
        public DateTime Date { get; set; }
        //TODO: в рамках тестового задания не делал контроль на время
        // т.е. время начала дожно быть меньше времени окончания и т.д.
        // - так же не делал контроль на перекрестное время занятий / преподавателей / груп
        // - так же не делал контроль если выбрана дисциплина не принадлежащая преподавателю
        public DateTime TimeBegin { get; set; }
        public DateTime TimeEnd { get; set; }
        public Teacher Teacher { get; set; }
        public Group Group { get; set; }
    }
}
