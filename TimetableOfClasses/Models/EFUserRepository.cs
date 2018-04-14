using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using TimetableOfClasses.Models.ViewModels;

namespace TimetableOfClasses.Models
{
    public class EFUserRepository : IUserRepository
    {
        private UserManager<IdentityUser> userManager;
        private IServiceProvider serviceProvider;

        public EFUserRepository(IServiceProvider serviceProvider, UserManager<IdentityUser> userManager)
        {
            this.serviceProvider = serviceProvider;
            this.userManager = userManager;
        }

        public IQueryable<IdentityUser> Users => userManager.Users;

        public async Task<string> GetRoleUser(IdentityUser user)
        {
            var roles = await userManager.GetRolesAsync(user);
            // т.к. роль у пользователя всего одна, то берем первую
            return (roles.Count == 0) ? "" : roles[0]; // string.Join(",", roles.ToArray());
        }

        public async Task<bool> IsUserExistsByName(string userName)
        {
            return await userManager.FindByNameAsync(userName) != null;
        }

        public async void SaveUser(UserViewModel userViewModel)
        {
            var scope = serviceProvider.CreateScope();
            using (IServiceScope scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var provider = scope.ServiceProvider;
                UserManager<IdentityUser> _userManager = provider.GetRequiredService<UserManager<IdentityUser>>();

                if (string.IsNullOrEmpty(userViewModel.Id))
                {
                    IdentityUser user = new IdentityUser { UserName = userViewModel.UserName };
                    IdentityResult result = await _userManager.CreateAsync(user, userViewModel.Password);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddToRoleAsync(user, userViewModel.SelectedRole);
                    }
                }
                else
                {
                    IdentityUser dbEntry = await _userManager.FindByIdAsync(userViewModel.Id);
                    if (dbEntry != null)
                    {
                        dbEntry.UserName = userViewModel.UserName;
                        IdentityResult result = await _userManager.RemoveFromRolesAsync(dbEntry, await _userManager.GetRolesAsync(dbEntry));
                        if (result.Succeeded)
                        {
                            result = await _userManager.AddToRoleAsync(dbEntry, userViewModel.SelectedRole);
                            if (result.Succeeded)
                            {
                                result = await _userManager.UpdateAsync(dbEntry);
                            }
                        }
                    }
                }
            }
        }

        public async Task<IdentityUser> ChangePasswordAsync(ChangePasswordViewModel user)
        {
            var dbEntry = await userManager.FindByIdAsync(user.Id);
            if (dbEntry != null)
            {
                IdentityResult result = await userManager.ChangePasswordAsync(dbEntry, user.OldPassword, user.NewPassword);
                if (!result.Succeeded)
                {
                    dbEntry = null;
                }
            }
            return dbEntry;
        }

        public async Task<IdentityUser> DeleteUserAsync(string userId)
        {
            IdentityUser dbEntry = await userManager.FindByIdAsync(userId);
            if (dbEntry != null)
            {
                IdentityResult result = await userManager.DeleteAsync(dbEntry);
            }
            return dbEntry;
        }
    }
}
