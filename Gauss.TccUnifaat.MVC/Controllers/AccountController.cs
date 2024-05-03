using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Common.Services.Interfaces;
using Gauss.TccUnifaat.Data;
using Gauss.TccUnifaat.MVC.Extensions;
using Gauss.TccUnifaat.MVC.ViewModels;
using Gauss.TccUnifaat.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Gauss.TccUnifaat.Controllers
{
    public class AccountController : ControllerBase<ApplicationDbContext, RT.Comb.ICombProvider>
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<Funcao> _roleManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(ApplicationDbContext context
            , RT.Comb.ICombProvider comb
            , UserManager<Usuario> userManager
            , RoleManager<Funcao> roleManager
            , SignInManager<Usuario> signInManager
            , IEmailService emailService
            ) : base(context, comb)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailService = emailService;
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
                    ModelState.AddModelError(string.Empty, "O usuário não foi encontrado na base de dados.");
                    this.MostrarMensagem($"O usuário não foi encontrado na base de dados.", erro: true);
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (await _userManager.IsInRoleAsync(user, "Administrador"))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else if (await _userManager.IsInRoleAsync(user, "Professor") || await _userManager.IsInRoleAsync(user, "Aluno"))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Portal" });
                    }

                    var sessionId = this._comb.Create();

                    await this._context.SaveChangesAsync();
                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError(string.Empty, "Login ou senha inválida.");
                this.MostrarMensagem($"Login ou senha inválida.", erro: true);
                return View(model);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "home");
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
        public IActionResult EsqueciSenha()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EsqueciSenha(EsqueciSenhaViewModel dados)
        {
            if (ModelState.IsValid)
            {
                if (_userManager.Users.AsNoTracking().Any(u => u.NormalizedEmail == dados.Email.ToUpper().Trim()))
                {
                    var usuario = await _userManager.FindByEmailAsync(dados.Email);
                    var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
                    var urlConfirmacao = Url.Action(nameof(RedefinirSenha), "Account", new { token }, Request.Scheme);
                    var mensagem = new StringBuilder();
                    mensagem.Append($"<p>Olá, {usuario.NomeCompleto}.</p>");
                    mensagem.Append("<p>Houve uma solicitação de redefinição de senha para seu usuário em nosso site. Se não foi você que fez a solicitação, ignore essa mensagem. Caso tenha sido você, clique no link abaixo para criar sua nova senha:</p>");
                    mensagem.Append($"<p><a href='{urlConfirmacao}'>Redefinir Senha</a></p>");
                    mensagem.Append("<p>Atenciosamente,<br>Equipe de Suporte</p>");
                    await _emailService.SendEmailAsync(usuario.Email,
                        "Redefinição de Senha", "", mensagem.ToString());
                    return View(nameof(EmailRedefinicaoEnviado));
                }
                else
                {
                    this.MostrarMensagem($"E-mail {dados.Email} não encontrado.", erro: true);

                    return View();
                }
            }
            else
            {
                return View(dados);
            }
        }

        [AllowAnonymous]
        public IActionResult EmailRedefinicaoEnviado()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RedefinirSenha(string token)
        {
            var modelo = new RedefinirSenhaViewModel();
            modelo.Token = token;
            return View(modelo);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RedefinirSenha(RedefinirSenhaViewModel dados)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _userManager.FindByEmailAsync(dados.Email);
                var resultado = await _userManager.ResetPasswordAsync(
                    usuario, dados.Token, dados.NovaSenha);
                if (resultado.Succeeded)
                {
                    this.MostrarMensagem(
                       $"Senha redefinida com sucesso! Agora você já pode fazer login com a nova senha.");
                    return View();
                }
                else
                {
                    this.MostrarMensagem($"Não foi possível redefinir a senha. Verifique se preencheu a senha corretamente. Se o problema persistir, entre em contato com o suporte.", erro: true);
                    return View(dados);
                }
            }
            else
            {
                return View(dados);
            }
        }


        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
