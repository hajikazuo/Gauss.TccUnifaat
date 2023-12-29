using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Data;
using NuGet.Packaging;

namespace Gauss.TccUnifaat.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TurmasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TurmasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Turmas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Turmas.ToListAsync());
        }

        // GET: Admin/Turmas/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turma = await _context.Turmas
                .Include(t => t.Usuarios) 
                .FirstOrDefaultAsync(m => m.TurmaId == id);

            if (turma == null)
            {
                return NotFound();
            }

            return View(turma);
        }

        // GET: Admin/Turmas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Turmas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TurmaId,Nome")] Turma turma)
        {
            if (ModelState.IsValid)
            {
                turma.TurmaId = Guid.NewGuid();
                _context.Add(turma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(turma);
        }

        // GET: Admin/Turmas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turma = await _context.Turmas.FindAsync(id);
            if (turma == null)
            {
                return NotFound();
            }
            return View(turma);
        }

        // POST: Admin/Turmas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TurmaId,Nome")] Turma turma)
        {
            if (id != turma.TurmaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turma);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurmaExists(turma.TurmaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(turma);
        }

        // GET: Admin/Turmas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turma = await _context.Turmas
                .FirstOrDefaultAsync(m => m.TurmaId == id);
            if (turma == null)
            {
                return NotFound();
            }

            return View(turma);
        }

        // POST: Admin/Turmas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var turma = await _context.Turmas.FindAsync(id);
            if (turma != null)
            {
                _context.Turmas.Remove(turma);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TurmaExists(Guid id)
        {
            return _context.Turmas.Any(e => e.TurmaId == id);
        }

        public async Task<IActionResult> AdicionarUsuarios(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turma = await _context.Turmas.Include(t => t.Usuarios).FirstOrDefaultAsync(m => m.TurmaId == id);
            if (turma == null)
            {
                return NotFound();
            }

            var usuariosDaTurma = turma.Usuarios;

            var todosUsuarios = await _context.Usuarios.ToListAsync();

            var usuariosDisponiveis = todosUsuarios.Except(usuariosDaTurma).ToList();

            ViewData["UsuariosDisponiveis"] = usuariosDisponiveis;
            ViewData["UsuariosDaTurma"] = usuariosDaTurma;

            return View(turma);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarUsuarios(Guid id, string[] usuariosSelecionados)
        {
            var turma = await _context.Turmas.FindAsync(id);
            if (turma == null)
            {
                return NotFound();
            }

            turma.Usuarios?.Clear();

            if (usuariosSelecionados != null)
            {
                var usuariosSelecionadosGuid = usuariosSelecionados.Select(Guid.Parse).ToList();

                var usuariosAssociados = await _context.Usuarios
                    .Where(u => usuariosSelecionadosGuid.Contains(u.Id))
                    .ToListAsync();

                turma.Usuarios ??= new List<Usuario>();
                turma.Usuarios.AddRange(usuariosAssociados);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(AdicionarUsuarios), new { id });
        }

        public async Task<IActionResult> RemoverUsuario(Guid turmaId, Guid usuarioId)
        {
            var turma = await _context.Turmas.Include(t => t.Usuarios).FirstOrDefaultAsync(m => m.TurmaId == turmaId);

            if (turma != null)
            {
                var usuarioParaRemover = turma.Usuarios.FirstOrDefault(u => u.Id == usuarioId);

                if (usuarioParaRemover != null)
                {
                    turma.Usuarios.Remove(usuarioParaRemover);

                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Details", new { id = turmaId });
        }




    }
}
