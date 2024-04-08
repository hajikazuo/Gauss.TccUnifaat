using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Gauss.TccUnifaat.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Gauss.TccUnifaat.Controllers;
using Gauss.TccUnifaat.MVC.Extensions;

namespace Gauss.TccUnifaat.MVC.Areas.Portal.Controllers
{
    [Area("Portal")]
    public class AvisosController : ControllerBase<ApplicationDbContext, RT.Comb.ICombProvider>
    {
        public AvisosController(ApplicationDbContext context
            , RT.Comb.ICombProvider comb
            ) : base(context, comb)
        {
        }

        // GET: Portal/Avisos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Avisos.Include(a => a.Turma);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Portal/Avisos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aviso = await _context.Avisos
                .Include(a => a.Turma)
                .FirstOrDefaultAsync(m => m.AvisoId == id);
            if (aviso == null)
            {
                return NotFound();
            }

            return View(aviso);
        }

        // GET: Portal/Avisos/Create
        public IActionResult Create()
        {
            var disciplinas = _context.Disciplinas.ToList();

            if (disciplinas.Count == 0)
            {
                this.MostrarMensagem($"Não há disciplinas cadastradas. Por favor, cadastre uma disciplina antes de adicionar avisos.", erro: true);
                return RedirectToAction(nameof(Index));
            }

            ViewData["TurmaId"] = new SelectList(_context.Turmas, "TurmaId", "Nome");
            return View();
        }

        // POST: Portal/Avisos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AvisoId,Titulo,Descricao,DataAviso,TurmaId")] Aviso aviso)
        {
            if (ModelState.IsValid)
            {
                aviso.AvisoId = _comb.Create();
                _context.Add(aviso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TurmaId"] = new SelectList(_context.Turmas, "TurmaId", "Nome", aviso.TurmaId);
            return View(aviso);
        }

        // GET: Portal/Avisos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aviso = await _context.Avisos.FindAsync(id);
            if (aviso == null)
            {
                return NotFound();
            }
            ViewData["TurmaId"] = new SelectList(_context.Turmas, "TurmaId", "Nome", aviso.TurmaId);
            return View(aviso);
        }

        // POST: Portal/Avisos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AvisoId,Titulo,Descricao,DataAviso,TurmaId,DataCadastro")] Aviso aviso)
        {
            if (id != aviso.AvisoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aviso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AvisoExists(aviso.AvisoId))
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
            ViewData["TurmaId"] = new SelectList(_context.Turmas, "TurmaId", "Nome", aviso.TurmaId);
            return View(aviso);
        }

        // GET: Portal/Avisos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aviso = await _context.Avisos
                .Include(a => a.Turma)
                .FirstOrDefaultAsync(m => m.AvisoId == id);
            if (aviso == null)
            {
                return NotFound();
            }

            return View(aviso);
        }

        // POST: Portal/Avisos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var aviso = await _context.Avisos.FindAsync(id);
            if (aviso != null)
            {
                _context.Avisos.Remove(aviso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AvisoExists(Guid id)
        {
            return _context.Avisos.Any(e => e.AvisoId == id);
        }
    }
}
