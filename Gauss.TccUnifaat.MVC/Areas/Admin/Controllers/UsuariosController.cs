﻿using Dapper;
using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Controllers;
using Gauss.TccUnifaat.Data;
using Gauss.TccUnifaat.MVC.Dapper;
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
    public class UsuariosController : ControllerBase<ApplicationDbContext, RT.Comb.ICombProvider>
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

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        //[HttpGet]
        //public IActionResult Register()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Register(Guid? id)
        {
            if (id.HasValue)
            {
                var usuarioBD = await _userManager.FindByIdAsync(id.ToString());
                if (usuarioBD == null)
                {
                    this.MostrarMensagem($"O usuário não foi encontrado na base de dados.", erro: true);
                    return RedirectToAction("Index", "Usuarios");
                }

                var usuarioVM = new RegisterViewModel
                {
                    NomeCompleto = usuarioBD.NomeCompleto,
                    DataNascimento = usuarioBD.DataNascimento,
                    Cpf = usuarioBD.Cpf,
                    Email = usuarioBD.Email,
                    Telefone = usuarioBD.Telefone
                };

                return View(usuarioVM);
            }
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, Guid? id)
        {
            if (id.HasValue)
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }

            if (ModelState.IsValid)
            {
                Usuario user;
                if (id != null && EntidadeExiste(id.Value))
                {
                    user = await _userManager.FindByIdAsync(id.Value.ToString());
                    if (user == null)
                    {
                        return NotFound();
                    }

                    user.UserName = model.Email;
                    user.Email = model.Email;
                    user.NomeCompleto = model.NomeCompleto;
                    user.Cpf = model.Cpf;
                    user.Telefone = model.Telefone;
                    user.DataNascimento = model.DataNascimento;
                }
                else
                {
                    user = new Usuario
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        NomeCompleto = model.NomeCompleto,
                        Cpf = model.Cpf,
                        Telefone = model.Telefone,
                        DataNascimento = model.DataNascimento,
                        EmailConfirmed = true
                    };
                }

                var result = id != null ? await _userManager.UpdateAsync(user) : await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (model.SelectedRole != null)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        await _userManager.RemoveFromRolesAsync(user, userRoles);

                        await _userManager.AddToRoleAsync(user, model.SelectedRole);
                    }


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
