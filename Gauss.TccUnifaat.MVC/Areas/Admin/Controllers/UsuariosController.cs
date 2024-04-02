using Dapper;
using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Data;
using Gauss.TccUnifaat.MVC.Extensions;
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
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        public UsuariosController(ApplicationDbContext context
            , RT.Comb.ICombProvider comb, UserManager<Usuario> userManager, SignInManager<Usuario> signInManager
            ) : base(context, comb)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                this.MostrarMensagem("Usuário não informado.", true);
                return RedirectToAction(nameof(Index));
            }

            if (!EntidadeExiste(id.Value))
            {
                this.MostrarMensagem("Usuário não encontrado.", true);
                return RedirectToAction(nameof(Index));
            }

            var usuario = await _userManager.FindByIdAsync(id.ToString());

            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var usuario = await _userManager.FindByIdAsync(id.ToString());
            if (usuario != null)
            {
                var resultado = await _userManager.DeleteAsync(usuario);
                if (resultado.Succeeded)
                {
                    this.MostrarMensagem("Usuário excluído com sucesso.");
                }
                else
                {
                    this.MostrarMensagem("Não foi possível excluir o usuário.", true);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                this.MostrarMensagem("Usuário não encontrado.", true);
                return RedirectToAction(nameof(Index));
            }
        }

        private bool EntidadeExiste(Guid id)
        {
            return (_userManager.Users.AsNoTracking().Any(u => u.Id == id));
        }

    }
}
