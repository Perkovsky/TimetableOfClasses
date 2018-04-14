using System.Linq;

namespace TimetableOfClasses.Models
{
    public interface ITimetableRepository
    {
        IQueryable<Timetable> Timetables { get; }

        void SaveTimetable(Timetable tеimetable);

        Timetable DeleteTimetable(int timetableId);
    }
}
