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
        public IActionResult RelatorioExemplo1()
        {
            var sqlUsuarios = Common.Resources.querys.usuarios_dashboard;
            var conn2 = _context.Database.GetDbConnection();
            var usuarios = conn2.Query<DashboardUsuariosViewModel>(sqlUsuarios);

            ViewBag.Usuarios = usuarios;

            return  new ViewAsPdf(usuarios);
        }
    }
}
