using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Controllers;
using Gauss.TccUnifaat.Data;
using Gauss.TccUnifaat.MVC.ViewModels;
using Gauss.TccUnifaat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gauss.TccUnifaat.MVC.Areas.Portal.Controllers
{
    [Area("Portal")]

    public class HomeController : ControllerBase<ApplicationDbContext, RT.Comb.ICombProvider>
    {
        private readonly UserManager<Usuario> _userManager;
        public HomeController(ApplicationDbContext context
           , RT.Comb.ICombProvider comb, UserManager<Usuario> userManager
           ) : base(context, comb)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var avisos = await _context.Avisos
                .Where(aviso => aviso.TurmaId == currentUser.TurmaId)
                .ToListAsync();

            var disciplinasNaTurma = await _context.Disciplinas
                .Where(d => d.TurmaId == currentUser.TurmaId)
                .Select(d => d.DisciplinaId)
                .ToListAsync();
 
            var horarios = await _context.Horarios
                .Include(h => h.Usuario)
                .Include(h => h.Disciplina)
                .Where(horario => disciplinasNaTurma.Contains(horario.DisciplinaId))
                .OrderBy(horario => horario.DataAula)
                .ToListAsync();

            var avisosViewModel = avisos.Select(aviso => new AvisosViewModel
            {
                Titulo = aviso.Titulo,
                Descricao = aviso.Descricao,
                DataAviso = aviso.DataAviso,
            }).ToList();

            var horariosViewModel = horarios.Select(horario => new HorarioViewModel
            {
                Usuario = horario.Usuario.NomeCompleto,
                Telefone = horario.Usuario.Telefone,
                Email = horario.Usuario.Email,
                Disciplina = horario.Disciplina.Nome,
                DataAula = horario.DataAula
            }).ToList();

            var viewModel = new HomePortalViewModel
            {
                Avisos = avisosViewModel,
                Horarios = horariosViewModel
            };

            return View(viewModel);

        }
    }
}
