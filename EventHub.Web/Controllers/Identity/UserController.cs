using EventHub.Web.Models.Identity.Roles;
using EventHub.Web.Models.Identity.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Web.Controllers.Identity
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<IdentityUser> userManager , RoleManager<IdentityRole> roleManager)
        {
           _userManager = userManager;
           _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync(); 
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); 
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email!,
                    Username = user.UserName!,
                    Roles = roles
                });
            }

            return View(userViewModels);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string Id )
        
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user is null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.ToListAsync();

            var userModel = new UserRoleViewModel
            {
                UserId = user.Id,
                Username = user.UserName!,
                Roles = roles.Select(role => new UpdateRoleViewModel
                {

                    Id = role.Id,
                    Name = role.Name!,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name!).GetAwaiter().GetResult()
                }).ToList()
            };


            return View(userModel);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(UserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user is null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if(roles.Any(currentRole => currentRole == role.Name) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.Name);

                if (!roles.Any(currentRole => currentRole == role.Name) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.Name);
            }


            return RedirectToAction(nameof(Index));
        }
    }
}
