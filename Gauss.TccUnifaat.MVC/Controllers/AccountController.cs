using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Data;

using Gauss.TccUnifaat.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gauss.TccUnifaat.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<Funcao> _roleManager;
        private readonly SignInManager<Usuario> _signInManager;

        public AccountController(ApplicationDbContext context
            , RT.Comb.ICombProvider comb
            , UserManager<Usuario> userManager
            , RoleManager<Funcao> roleManager
            , SignInManager<Usuario> signInManager) : base(context, comb)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null, string loginName = null)
        {
            ViewBag.Confirm = TempData["Confirm"];
            ViewData["ReturnUrl"] = returnUrl;

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "CPF ou senha inválida.");
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (await _userManager.IsInRoleAsync(user, "Administrador"))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }

                    var sessionId = this._comb.Create();

                    await this._context.SaveChangesAsync();
                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError(string.Empty, "CPF ou senha inválida.");
                return View(model);
            }

            return View(model);
        }

        private string GetIP()
        {
            var ip = HttpContext.Connection?.RemoteIpAddress?.ToString();
            if (ip == "::1" || ip == null)
            {
                ip = HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAdmin()
        {
            var existingAdmin = await _userManager.FindByEmailAsync("gauss@gauss.com.br");
            if (existingAdmin != null)
            {
                return Content("Já existe um administrador.");
            }

            var adminUser = new Usuario
            {
                Id = _comb.Create(),
                UserName = "gauss@gauss.com.br",
                NomeCompleto = "gauss admin",
                Email = "gauss@gauss.com.br",
                Cpf = "12345678910",
                Telefone = "11911112222",
                Idade = 30,
                EmailConfirmed = true,
            };

            var createResult = await _userManager.CreateAsync(adminUser, "Gauss@2023");

            if (createResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Administrador");
                return Content("Usuário administrador criado com sucesso.");
            }

            foreach (var error in createResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Content("Erro ao criar o usuário administrador.");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
