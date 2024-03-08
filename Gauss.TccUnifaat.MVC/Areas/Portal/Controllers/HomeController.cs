using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Data;
using Gauss.TccUnifaat.MVC.ViewModels;
using Gauss.TccUnifaat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gauss.TccUnifaat.MVC.Areas.Portal.Controllers
{
    [Area("Portal")]

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        public HomeController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var avisos = await _context.Avisos
            .Where(aviso => aviso.TurmaId == currentUser.TurmaId)
            .ToListAsync();

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
