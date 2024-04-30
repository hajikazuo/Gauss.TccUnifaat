using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Common.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Gauss.TccUnifaat.Common.Services
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
            if (_userManager.FindByEmailAsync("usuario@gauss.com.br").Result == null)
            {
                Usuario user = new Usuario();
                user.UserName = "usuario@gauss.com.br";
                user.Email = "usuario@gauss.com.br";
                user.NormalizedUserName = "USUARIO@GAUSS.COM.BR";
                user.NormalizedEmail = "USUARIO@GAUSS.COM.BR";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.NomeCompleto = "aluno teste";
                user.Cpf = "111.111.111-11";
                user.Telefone = "(11) 91111-1111";
                user.DataNascimento = new DateTime(2024, 1, 1);
                user.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult result = _userManager.CreateAsync(user, "Gauss@2024").Result;

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Aluno").Wait();
                }
            }

            if (_userManager.FindByEmailAsync("professor@gauss.com.br").Result == null)
            {
                Usuario user = new Usuario();
                user.UserName = "professor@gauss.com.br";
                user.Email = "professor@gauss.com.br";
                user.NormalizedUserName = "PROFESSOR@GAUSS.COM.BR";
                user.NormalizedEmail = "PROFESSOR@GAUSS.COM.BR";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.NomeCompleto = "professor teste";
                user.Cpf = "222.222.222-22";
                user.Telefone = "(11) 92222-2222";
                user.DataNascimento = new DateTime(2024, 1, 1);
                user.SecurityStamp = Guid.NewGuid().ToString(); 

                IdentityResult result = _userManager.CreateAsync(user, "Gauss@2024").Result;

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
                user.Cpf = "333.333.333-33";
                user.Telefone = "(11) 93333-3333";
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
