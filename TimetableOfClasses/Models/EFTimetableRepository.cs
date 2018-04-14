using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TimetableOfClasses.Models
{
    public class EFTimetableRepository : ITimetableRepository
    {
        private ApplicationDbContext context;

        public EFTimetableRepository(ApplicationDbContext context) => this.context = context;

        public IQueryable<Timetable> Timetables => context.Timetables.Include(t => t.Discipline)
                                                                     .Include(t => t.Teacher)
                                                                     .Include(t => t.Group);

        public void SaveTimetable(Timetable timetable)
        {
            if (timetable.TimetableId == 0)
            {
                context.Timetables.Add(timetable);
            }
            else
            {
                Timetable dbEntry = context.Timetables.FirstOrDefault(t => t.TimetableId == timetable.TimetableId);
                if (dbEntry != null)
                {
                    dbEntry.Discipline = timetable.Discipline;
                    dbEntry.Date = timetable.Date;
                    dbEntry.TimeBegin = timetable.TimeBegin;
                    dbEntry.TimeEnd = timetable.TimeEnd;
                    dbEntry.Teacher = timetable.Teacher;
                    dbEntry.Group = timetable.Group;
                }
            }
            context.SaveChanges();
        }

        public Timetable DeleteTimetable(int timetableId)
        {
            Timetable dbEntry = context.Timetables.FirstOrDefault(t => t.TimetableId == timetableId);
            if (dbEntry != null)
            {
                context.Timetables.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
