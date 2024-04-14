using Dapper;
using Gauss.TccUnifaat.Data;
using Gauss.TccUnifaat.MVC.Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

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

            return  new ViewAsPdf(usuarios);
        }

        public IActionResult RelatorioUsuariosPorTurma()
        {
            var sqlUsuariosPorTurma = Common.Resources.querys.usuariosPorTurma_dashboard;
            var conn = _context.Database.GetDbConnection();
            var usuarios = conn.Query<UsuariosViewModel>(sqlUsuariosPorTurma);

            ViewBag.Usuarios = usuarios;

            return new ViewAsPdf(usuarios);
        }

        public IActionResult RelatorioFaltas()
        {
            var sqlControleFaltas = Common.Resources.querys.controle_faltas;
            var conn = _context.Database.GetDbConnection();
            var faltas = conn.Query<ControleFaltasViewModel>(sqlControleFaltas);

            ViewBag.Usuarios = faltas;

            return new ViewAsPdf(faltas);
        }
    }
}
