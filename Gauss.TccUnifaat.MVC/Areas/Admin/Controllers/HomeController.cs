using Dapper;
using Gauss.TccUnifaat.Data;
using Gauss.TccUnifaat.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gauss.TccUnifaat.MVC.Areas.Admin.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    [Area("Admin")]

    public class HomeController : Controller
    {
        protected readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //Dapper
            var sqlNoticias = Gauss.TccUnifaat.Common.Resources.querys.noticias_dashboard;
            var conn = _context.Database.GetDbConnection();
            var noticias = conn.Query<DashboardNoticiasViewModel>(sqlNoticias);

            var sqlUsuarios = Gauss.TccUnifaat.Common.Resources.querys.usuarios_dashboard;
            var conn2 = _context.Database.GetDbConnection();
            var usuarios = conn2.Query<DashboardUsuariosViewModel>(sqlUsuarios);

            ViewBag.Usuarios = usuarios;

            return View(noticias);
        }
    }
}
