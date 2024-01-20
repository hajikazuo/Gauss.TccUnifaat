using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Data;
using Microsoft.AspNetCore.Identity;

namespace Gauss.TccUnifaat.MVC.Areas.Portal.Controllers
{
    [Area("Portal")]
    public class TurmasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public TurmasController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var turmaDoUsuario = await _context.Turmas
            .Include(t => t.Usuarios)
            .Where(t => t.Usuarios.Any(u => u.Id == currentUser.Id))
            .ToListAsync();

            return View(turmaDoUsuario);
        }

        [HttpPost]
        public async Task<IActionResult> MarcarPresenca([FromForm] DateTime dataAula, [FromForm] Dictionary<string, bool> presenca)
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

            TempData["PresencaSalva"] = true;

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ListaPresencas(Guid turmaId)
        {
            var presencasNaTurma = await _context.Presencas
                .Where(p => p.TurmaId == turmaId)
                .Include(p => p.Usuario)
                .OrderBy(p => p.DataAula)
                .ToListAsync();

            return View(presencasNaTurma);
        }


    }
}
