using Dapper;
using Gauss.TccUnifaat.Data;
using Gauss.TccUnifaat.MVC.Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Web;

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
            var sqlNoticias = Common.Resources.querys.noticias_dashboard;
            var conn = _context.Database.GetDbConnection();
            var noticias = conn.Query<DashboardNoticiasViewModel>(sqlNoticias);

            var sqlUsuarios = Common.Resources.querys.usuarios_dashboard;
            var conn2 = _context.Database.GetDbConnection();
            var usuarios = conn2.Query<DashboardUsuariosViewModel>(sqlUsuarios);

            var sqlUsuariosPorTurma = Common.Resources.querys.usuariosPorTurma_dashboard;
            var conn3 = _context.Database.GetDbConnection();
            var usuariosPorTurma = conn3.Query<UsuariosViewModel>(sqlUsuariosPorTurma);

            foreach (var usuario in usuariosPorTurma)
            {
                usuario.Turma = RemoverAcentos(usuario.Turma);
            }

            ViewBag.Noticias = noticias;
            ViewBag.Usuarios = usuarios;
            ViewBag.UsuariosPorTurma = usuariosPorTurma;

            return View();
        }

        public string RemoverAcentos(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(texto);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
