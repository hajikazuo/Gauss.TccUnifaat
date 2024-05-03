using Dapper;
using Gauss.TccUnifaat.Data;
using Gauss.TccUnifaat.MVC.Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gauss.TccUnifaat.MVC.Areas.Admin.Controllers
{

    [Authorize(Policy = "RequireAdminRole")]
    [Area("Admin")]

    public class RelatoriosController : Controller
    {
        protected readonly ApplicationDbContext _context;

        public RelatoriosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RelatorioQtdFuncao()
        {
            var sqlUsuarios = Common.Resources.querys.usuarios_dashboard;
            var conn = _context.Database.GetDbConnection();
            var usuarios = conn.Query<DashboardUsuariosViewModel>(sqlUsuarios);

            ViewBag.Usuarios = usuarios;

            return View(usuarios);
        }

        public IActionResult RelatorioUsuariosPorTurma()
        {
            var sqlUsuariosPorTurma = Common.Resources.querys.usuariosPorTurma_dashboard;
            var conn = _context.Database.GetDbConnection();
            var usuarios = conn.Query<UsuariosViewModel>(sqlUsuariosPorTurma);

            ViewBag.Usuarios = usuarios;

            return View(usuarios);
        }

        public async Task<IActionResult> RelatorioFaltasAsync(DateTime? dataFiltro = null)
        {
            var dataAtual = DateTime.Today;

            if (dataFiltro == null)
            {
                dataFiltro = dataAtual;
            }

            var faltasPorUsuario = await _context.Presencas
                .Include(p => p.Usuario)
                .Include(p => p.Turma)
                .Where(p => p.Presente != true && p.DataAula.Date.Month == dataFiltro.Value.Date.Month)
                .GroupBy(p => new { p.UsuarioId, p.Usuario.NomeCompleto, p.Turma.TurmaId, p.Turma.Nome })
                .Select(g => new ControleFaltasViewModel
                {
                    UsuarioId = g.Key.UsuarioId,
                    NomeCompleto = g.Key.NomeCompleto,
                    TurmaId = g.Key.TurmaId,
                    NomeTurma = g.Key.Nome,
                    QtdeFaltas = g.Count()
                })
                .OrderByDescending(g => g.QtdeFaltas)
                .ToListAsync();

            ViewBag.DataAtual = dataAtual;
            ViewBag.DataFiltro = dataFiltro;
            return View(faltasPorUsuario);
        }

    }
}
