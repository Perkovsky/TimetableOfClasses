using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;

namespace TimetableOfClasses.Models
{
    public class EFGroupRepository : IGroupRepository
    {
        private ApplicationDbContext context;

        public EFGroupRepository(ApplicationDbContext context) => this.context = context;

        public IQueryable<Group> Groups => context.Groups;

        public void SaveGroup(Group group)
        {
            if (group.GroupId == 0)
            {
                context.Groups.Add(group);
            }
            else
            {
                Group dbEntry = context.Groups.FirstOrDefault(g => g.GroupId == group.GroupId);
                if (dbEntry != null)
                {
                    dbEntry.Name = group.Name;
                    dbEntry.DateBeginAction = group.DateBeginAction;
                    dbEntry.DateEndAction = group.DateEndAction;
                }
            }
            context.SaveChanges();
        }

        public Group DeleteGroup(int groupId)
        {
            Group dbEntry = context.Groups.FirstOrDefault(g => g.GroupId == groupId);
            if (dbEntry != null)
            {
                context.Groups.Remove(dbEntry);
                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    throw new Exception("Error Delete! The Group is present in the table of Timetable");
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
