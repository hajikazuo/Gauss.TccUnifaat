using Dapper;
using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Data;
using Gauss.TccUnifaat.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;

namespace Gauss.TccUnifaat.MVC.Areas.Admin.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    [Area("Admin")]
    public class UsuariosController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        public readonly ApplicationDbContext _context;

        public UsuariosController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sqlUsuarios = Common.Resources.querys.usuarios;
            var conn = _context.Database.GetDbConnection();
            var Usuarios = conn.Query<UsuariosViewModel>(sqlUsuarios);

            return View(Usuarios);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Usuario
                {
                    UserName = model.Email,
                    Email = model.Email,
                    NomeCompleto = model.NomeCompleto,
                    Cpf = model.Cpf,
                    Telefone = model.Telefone,
                    Idade = model.Idade,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.SelectedRole);

                    return RedirectToAction("Index", new { area = "Admin" });

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


    }
}
