using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TimetableOfClasses.Models.ViewModels;

namespace TimetableOfClasses.Models
{
    public class AppIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public const string ADMIN = "ADMIN";
        public const string STUDENT = "STUDENT";

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options) { }

        private static async Task AddRole(RoleManager<IdentityRole> roleManager, string role)
        {
            if (await roleManager.FindByNameAsync(role) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        public static async Task CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await AddRole(roleManager, ADMIN);
            await AddRole(roleManager, STUDENT);

            if (!(await userRepository.IsUserExistsByName(ADMIN)))
            {
                var userViewModel = new UserViewModel
                {
                    UserName = ADMIN,
                    SelectedRole = ADMIN,
                    Password = "Admin123$"
                };
                userRepository.SaveUser(userViewModel);
            }
        }
    }
}
