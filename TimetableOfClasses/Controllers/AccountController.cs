using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimetableOfClasses.Models;
using TimetableOfClasses.Models.ViewModels;

namespace TimetableOfClasses.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        public ViewResult Login() => View(new LoginViewModel());

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByNameAsync(loginViewModel.Name);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    if ((await signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false)).Succeeded)
                    {
                        if (await userManager.IsInRoleAsync(user, AppIdentityDbContext.ADMIN))
                        {
                            return Redirect("/Admin/Index");
                        }
                        else
                        {
                            return Redirect("/Home/Index");
                        }
                    }
                }
            }

            ModelState.AddModelError("", "Invalid name or password");
            return View(loginViewModel);
        }

        public async Task<RedirectResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect("/");
        }

        public IActionResult AccessDenied() => View();
    }
}
