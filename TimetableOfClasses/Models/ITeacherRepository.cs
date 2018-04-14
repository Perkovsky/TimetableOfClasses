using System.Collections.Generic;
using System.Linq;

namespace TimetableOfClasses.Models
{
    public interface ITeacherRepository
    {
        IQueryable<Teacher> Teachers { get; }

        void SaveTeacher(Teacher teacher, IList<Discipline> disciplines);

        Teacher DeleteTeacher(int TeacherId);
    }
}
