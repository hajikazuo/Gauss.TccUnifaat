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
            var sql = Gauss.TccUnifaat.Common.Resources.querys.noticias_dashboard;
            var conn = _context.Database.GetDbConnection();
            var noticias = conn.Query<DashboardViewModel>(sql);


            return View(noticias);
        }
    }
}
