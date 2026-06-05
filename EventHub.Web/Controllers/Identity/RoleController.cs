using EventHub.Web.Models.Identity.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Web.Controllers.Identity
{
    // Sistem rollerini (oluşturma, düzenleme, silme) yöneten controller
    // Yalnızca "Admin" rolüne sahip kullanıcılar bu controller'daki tüm işlemlere erişebilir
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        // ASP.NET Identity rol yönetimi işlemlerini gerçekleştiren servis
        private readonly RoleManager<IdentityRole> _roleManager;

        // Bağımlılık enjeksiyonu ile RoleManager örneği constructor aracılığıyla alınır
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // GET: /Role/Index — Sistemdeki tüm rolleri listeler
        public async Task<IActionResult> Index ()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        // POST: /Role/Create — Yeni bir rol oluşturur; aynı isimde rol varsa hata mesajı döner
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleExists = await _roleManager.RoleExistsAsync(model.Name);

                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Name));
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("Name", "Aynı isimde bir rol zaten mevcut.");
                return View(nameof(Index), await _roleManager.Roles.ToListAsync());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Role/Edit/{Id} — Belirtilen rolün düzenleme formunu gösterir; rol bulunamazsa listeye yönlendirir
        [HttpGet]
        public async Task<IActionResult> Edit (string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);


            if (role is null)
            {
                ModelState.AddModelError("Id", "Verilen kimliğe sahip bir rol bulunamadı.");
                return RedirectToAction(nameof(Index));
            }

            UpdateRoleViewModel mappedRole = new UpdateRoleViewModel
            {
                Id = Id,
                Name = role.Name!
            };

            return View(mappedRole);
        }


        // POST: /Role/Edit — Rolün adını günceller; aynı isimde başka rol varsa hata mesajı döner
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (UpdateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleExists = await _roleManager.RoleExistsAsync (model.Name);

                if (!roleExists)
                {
                    var role = await _roleManager.FindByIdAsync(model.Id);
                   if (role is not null)
                    {
                        role.Name = model.Name;
                        await _roleManager.UpdateAsync(role!);
                    }

                    return RedirectToAction(nameof(Index));


                }
                else
                {
                    ModelState.AddModelError("Name", "Aynı isimde bir rol zaten mevcut.");
                    return View(model);
                }
            }

            return RedirectToAction(nameof(Index));

        }

        // POST: /Role/Delete — Belirtilen rolü sistemden kalıcı olarak siler
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete (string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);

            if (role != null)
                await _roleManager.DeleteAsync(role);

            return RedirectToAction(nameof(Index));




        }
    }
}
