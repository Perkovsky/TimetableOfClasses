using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TimetableOfClasses.Models
{
    public class EFTeacherRepository : ITeacherRepository
    {
        private ApplicationDbContext context;

        public EFTeacherRepository(ApplicationDbContext context) => this.context = context;

        public IQueryable<Teacher> Teachers => context.Teachers.Include(t => t.TeacherDisciplines);

        private void AddDisciplines(int teacherId, IList<Discipline> disciplines)
        {
            foreach (var item in disciplines)
            {
                context.TeacherDisciplines.Add(new TeacherDiscipline
                {
                    TeacherId = teacherId,
                    DisciplineId = item.DisciplineId,
                });
            }
        }

        private void ClearDisciplines(int teacherId)
        {
            var delItems = context.TeacherDisciplines.Where(d => d.TeacherId == teacherId);
            context.TeacherDisciplines.RemoveRange(delItems);
            context.SaveChanges();
        }

        public void SaveTeacher(Teacher teacher, IList<Discipline> disciplines)
        {
            if (teacher.TeacherId == 0)
            {
                context.Teachers.Add(teacher);
            }
            else
            {
                Teacher dbEntry = context.Teachers.FirstOrDefault(t => t.TeacherId == teacher.TeacherId);
                if (dbEntry != null)
                {
                    dbEntry.Name = teacher.Name;
                    dbEntry.Phone = teacher.Phone;
                    ClearDisciplines(teacher.TeacherId);
                }
            }

            // add disciplines
            AddDisciplines(teacher.TeacherId, disciplines);
            context.SaveChanges();
        }

        public Teacher DeleteTeacher(int teacherId)
        {
            Teacher dbEntry = context.Teachers.FirstOrDefault(t => t.TeacherId == teacherId);
            if (dbEntry != null)
            {
                context.Teachers.Remove(dbEntry);
                try
                {
                    context.SaveChanges();
                    ClearDisciplines(teacherId);
                }
                catch (DbUpdateException)
                {
                    throw new Exception("Error Delete! The Teacher is present in the table of Timetable");
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
