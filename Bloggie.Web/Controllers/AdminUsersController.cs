using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<IdentityUser> userManager;

        public AdminUsersController(IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult>  List()
        {
            var users = await userRepository.GetAll();

            var userViewModel = new UserViewModel();

            userViewModel.Users = new List<User>();

            foreach(var user in users)
            {
                userViewModel.Users.Add(new Models.ViewModels.User()
                {
                    Id = Guid.Parse(user.Id),
                    UserName = user.UserName,
                    EmailAddress = user.Email,
                });
            }

            return View(userViewModel);
        }

        [HttpPost]

        public async Task<IActionResult> List(UserViewModel userViewModel)
        {
            var identityUser = new IdentityUser()
            {
                UserName = userViewModel.Username,
                Email  = userViewModel.Email,
            };

            var identityResult = await userManager.CreateAsync(identityUser, userViewModel.Password);

            if(identityResult is not null)
            {
                if (identityResult.Succeeded)
                {
                    // assign roles to new user
                    var roles = new List<string> { "User" }; // user role by default

                    if (userViewModel.AminRoleCheckbox) // if admin role checkbox is checked
                    {
                        roles.Add("Admin");
                    }

                    identityResult = await userManager.AddToRolesAsync(identityUser, roles);

                    if (identityResult is not null && identityResult.Succeeded)
                    {
                        return RedirectToAction("List", "AdminUsers");
                    }
                }
            }

            return View();
        }
    }
}
