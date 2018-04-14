using System.Linq;

namespace TimetableOfClasses.Models
{
    public interface IDisciplineRepository
    {
        IQueryable<Discipline> Disciplines { get; }

        void SaveDiscipline(Discipline discipline);

        Discipline DeleteDiscipline(int disciplineId);
    }
}
