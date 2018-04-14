using System.Linq;

namespace TimetableOfClasses.Models
{
    public interface IGroupRepository
    {
        IQueryable<Group> Groups { get; }

        void SaveGroup(Group group);

        Group DeleteGroup(int groupId);
    }
}
