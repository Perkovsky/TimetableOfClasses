using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using TimetableOfClasses.Models.ViewModels;

namespace TimetableOfClasses.Models
{
    public interface IUserRepository
    {
        IQueryable<IdentityUser> Users { get; }

        void SaveUser(UserViewModel user);

        Task<bool> IsUserExistsByName(string userName);

        Task<IdentityUser> ChangePasswordAsync(ChangePasswordViewModel user);

        Task<IdentityUser> DeleteUserAsync(string userId);

        Task<string> GetRoleUser(IdentityUser user);
    }
}
