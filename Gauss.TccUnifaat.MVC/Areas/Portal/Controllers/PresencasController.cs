using Dapper;
using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Controllers;
using Gauss.TccUnifaat.Data;
using Gauss.TccUnifaat.MVC.Dapper;
using Gauss.TccUnifaat.MVC.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gauss.TccUnifaat.MVC.Areas.Portal.Controllers
{
    [Area("Portal")]
    public class PresencasController : ControllerBase<ApplicationDbContext, RT.Comb.ICombProvider>
    {
        private readonly UserManager<Usuario> _userManager;
        public PresencasController(ApplicationDbContext context
            , RT.Comb.ICombProvider comb, UserManager<Usuario> userManager
            ) : base(context, comb)
        {
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminOrProfessorRole")]
        public async Task<IActionResult> Index(DateTime? dataFiltro = null)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var turmaIdDoUsuario = currentUser.TurmaId;
            var dataAtual = DateTime.Today;

            if (dataFiltro == null)
            {
                dataFiltro = dataAtual;
            }

            var presencasNaTurma = await _context.Presencas
                .Where(p => p.TurmaId == turmaIdDoUsuario && p.DataAula.Date == dataFiltro.Value.Date)
                .Include(p => p.Usuario)
                .OrderBy(p => p.DataAula)
                .ToListAsync();

            ViewBag.DataAtual = dataAtual;
            ViewBag.DataFiltro = dataFiltro;
            return View(presencasNaTurma);
        }

        [Authorize(Policy = "RequireAdminOrProfessorRole")]
        public async Task<IActionResult> ControleFaltas()
        {
            var sqlControleFaltas = Common.Resources.querys.controle_faltas;
            var conn = _context.Database.GetDbConnection();
            var faltas = conn.Query<ControleFaltasViewModel>(sqlControleFaltas);

            var currentUser = await _userManager.GetUserAsync(User);
            var turmaIdDoUsuario = currentUser.TurmaId;

            var controleFaltasNaTurma = faltas.Where(cf => cf.TurmaId == turmaIdDoUsuario);

            return View(controleFaltasNaTurma);
        }

        [Authorize(Policy = "RequireAdminOrProfessorRole")]
        public async Task<IActionResult> Create(DateTime dataAula)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var turmaDoUsuario = await _context.Turmas
                .Include(t => t.Usuarios)
                .Where(t => t.Usuarios.Any(u => u.Id == currentUser.Id))
                .ToListAsync();

            ViewData["DataAula"] = dataAula;
            return View(turmaDoUsuario);
        }

        [Authorize(Policy = "RequireAdminOrProfessorRole")]
        [HttpPost]
        public async Task<IActionResult> Create(DateTime dataAula, [FromForm] Dictionary<string, bool> presenca)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            foreach (var (usuarioId, presente) in presenca)
            {
                if (Guid.TryParse(usuarioId, out var parsedGuid))
                {
                    var novaPresenca = new Presenca
                    {
                        DataAula = dataAula,
                        Presente = presente,
                        TurmaId = currentUser.TurmaId ?? Guid.Empty,
                        UsuarioId = parsedGuid
                    };
                    _context.Presencas.Add(novaPresenca);
                }
            }

            await _context.SaveChangesAsync();

            this.MostrarMensagem("Presença salva com sucesso!.");

            return RedirectToAction(nameof(Create));
        }

        public async Task<IActionResult> ContagemFaltasPorUsuario()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var turmaIdDoUsuario = currentUser.TurmaId;

            var presencasDoUsuario = await _context.Presencas
                .Where(p => p.UsuarioId == currentUser.Id && p.TurmaId == turmaIdDoUsuario && !p.Presente)
                .ToListAsync();

            var totalFaltas = presencasDoUsuario.Sum(p => p.TotalFaltas);

            ViewBag.UserName = currentUser.NomeCompleto;

            return View(totalFaltas);
        }
    }
}
