using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Gauss.TccUnifaat.MVC.Services
{
    public class SeedUserRoleInitial : ISeedUserRoleInitial
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<Funcao> _roleManager;

        public SeedUserRoleInitial(UserManager<Usuario> userManager, RoleManager<Funcao> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedRoles()
        {
            if (!_roleManager.RoleExistsAsync("Aluno").Result)
            {
                Funcao role = new Funcao();
                role.Name = "Aluno";
                role.NormalizedName = "ALUNO";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }
            if (!_roleManager.RoleExistsAsync("Professor").Result)
            {
                Funcao role = new Funcao();
                role.Name = "Professor";
                role.NormalizedName = "PROFESSOR";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }
            if (!_roleManager.RoleExistsAsync("Administrador").Result)
            {
                Funcao role = new Funcao();
                role.Name = "Administrador";
                role.NormalizedName = "ADMINISTRADOR";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }
        }

        public void SeedUsers()
        {
            if (_userManager.FindByEmailAsync("usuario@localhost").Result == null)
            {
                Usuario user = new Usuario();
                user.UserName = "usuario@localhost";
                user.Email = "usuario@localhost";
                user.NormalizedUserName = "USUARIO@LOCALHOST";
                user.NormalizedEmail = "USUARIO@LOCALHOST";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.NomeCompleto = "aluno teste";
                user.Cpf = "11111111111";
                user.Telefone = "11911111111";
                user.DataNascimento = new DateTime(2024, 1, 1);
                user.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult result = _userManager.CreateAsync(user, "Gauss#2024").Result;

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Aluno").Wait();
                }
            }

            if (_userManager.FindByEmailAsync("professor@localhost").Result == null)
            {
                Usuario user = new Usuario();
                user.UserName = "professor@localhost";
                user.Email = "professor@localhost";
                user.NormalizedUserName = "PROFESSOR@LOCALHOST";
                user.NormalizedEmail = "PROFESSOR@LOCALHOST";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.NomeCompleto = "professor teste";
                user.Cpf = "22222222222";
                user.Telefone = "11922222222";
                user.DataNascimento = new DateTime(2024, 1, 1);
                user.SecurityStamp = Guid.NewGuid().ToString(); 

                IdentityResult result = _userManager.CreateAsync(user, "Gauss#2024").Result;

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Professor").Wait();
                }
            }

            if (_userManager.FindByEmailAsync("gauss@gauss.com.br").Result == null)
            {
                Usuario user = new Usuario();
                user.UserName = "gauss@gauss.com.br";
                user.Email = "gauss@gauss.com.br";
                user.NormalizedUserName = "GAUSS@GAUSS.COM.BR";
                user.NormalizedEmail = "GAUSS@GAUSS.COM.BR";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.NomeCompleto = "gauss admin";
                user.Cpf = "33333333333";
                user.Telefone = "11933333333";
                user.DataNascimento = new DateTime(2024, 1, 1);
                user.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult result = _userManager.CreateAsync(user, "Gauss@2024").Result;

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Administrador").Wait();
                }
            }
        }
    }
}
