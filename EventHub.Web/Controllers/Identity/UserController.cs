using EventHub.Web.Models.Identity.Roles;
using EventHub.Web.Models.Identity.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Web.Controllers.Identity
{
    // Kullanıcı listeleme ve kullanıcılara rol atama işlemlerini yöneten controller
    // Yalnızca "Admin" rolüne sahip kullanıcılar bu controller'daki tüm işlemlere erişebilir
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        // Kullanıcı sorgulama ve rol atama işlemlerini gerçekleştiren ASP.NET Identity servisi
        private readonly UserManager<IdentityUser> _userManager;
        // Sistemdeki rolleri sorgulayan ASP.NET Identity rol yönetim servisi
        private readonly RoleManager<IdentityRole> _roleManager;

        // Bağımlılık enjeksiyonu ile UserManager ve RoleManager örnekleri constructor aracılığıyla alınır
        public UserController(UserManager<IdentityUser> userManager , RoleManager<IdentityRole> roleManager)
        {
           _userManager = userManager;
           _roleManager = roleManager;
        }

        // GET: /User/Index — Sistemdeki tüm kullanıcıları ve her birinin atanmış rollerini listeler
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


        // GET: /User/Edit/{Id} — Belirtilen kullanıcı için rol atama formunu gösterir; mevcut roller işaretli olarak sunulur
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


        // POST: /User/Edit — Kullanıcının rol atamalarını günceller; seçili roller eklenir, kaldırılan roller çıkarılır
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
