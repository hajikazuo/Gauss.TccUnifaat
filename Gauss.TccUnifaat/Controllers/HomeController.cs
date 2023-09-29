using Gauss.TccUnifaat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Gauss.TccUnifaat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Usuario> _userManager;
        public RT.Comb.ICombProvider _comb;

        public HomeController(ILogger<HomeController> logger, UserManager<Usuario> userManager, RT.Comb.ICombProvider comb)
        {
            _logger = logger;
            _userManager = userManager;
            _comb = comb;
        }

        public IActionResult Index()
        {
            return View();
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
                NomeCompleto = "gauss projeto TCC",
                Email = "gauss@gauss.com.br",
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

    }
}