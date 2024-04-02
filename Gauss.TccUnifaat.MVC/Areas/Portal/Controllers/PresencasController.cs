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
using Gauss.TccUnifaat.Controllers;

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

            TempData["PresencaSalva"] = true;

            return RedirectToAction(nameof(Create));
        }

    }
}
