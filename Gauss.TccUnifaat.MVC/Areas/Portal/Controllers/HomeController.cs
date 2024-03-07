using Gauss.TccUnifaat.Data;
using Gauss.TccUnifaat.MVC.ViewModels;
using Gauss.TccUnifaat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gauss.TccUnifaat.MVC.Areas.Portal.Controllers
{
    [Area("Portal")]

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var avisos = _context.Avisos.ToList();

            var avisosViewModel = avisos.Select(aviso => new AvisosViewModel
            {
                Titulo = aviso.Titulo,
                Descricao = aviso.Descricao,
                DataAviso = aviso.DataAviso,
            }).ToList();

            return View(avisosViewModel);
        }
    }
}
