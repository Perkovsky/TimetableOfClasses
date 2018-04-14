using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;

namespace TimetableOfClasses.Models
{
    public class EFDisciplineRepository : IDisciplineRepository
    {
        private ApplicationDbContext context;

        public EFDisciplineRepository(ApplicationDbContext context) => this.context = context;

        public IQueryable<Discipline> Disciplines => context.Disciplines;

        public void SaveDiscipline(Discipline discipline)
        {
            if (discipline.DisciplineId == 0)
            {
                context.Disciplines.Add(discipline);
            }
            else
            {
                Discipline dbEntry = context.Disciplines.FirstOrDefault(d => d.DisciplineId == discipline.DisciplineId);
                if (dbEntry != null)
                {
                    dbEntry.Name = discipline.Name;
                }
            }
            context.SaveChanges();
        }

        public Discipline DeleteDiscipline(int disciplineId)
        {
            Discipline dbEntry = context.Disciplines.FirstOrDefault(d => d.DisciplineId == disciplineId);
            if (dbEntry != null)
            {
                context.Disciplines.Remove(dbEntry);
                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    throw new Exception("Error Delete! The Discipline is present in the table of Timetable");
                }
                catch (Exception e)
                {
                    // unknown exception
                    Debug.WriteLine(e.Message);
                    throw new Exception("Unknown exception");
                }
            }
            return dbEntry;
        }
    }
}
