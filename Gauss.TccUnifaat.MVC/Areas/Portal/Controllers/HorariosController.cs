using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Data;
using System.Security.Claims;
using Gauss.TccUnifaat.Controllers;
using Gauss.TccUnifaat.MVC.Extensions;

namespace Gauss.TccUnifaat.MVC.Areas.Portal.Controllers
{
    [Area("Portal")]
    public class HorariosController : ControllerBase<ApplicationDbContext, RT.Comb.ICombProvider>
    {
        public HorariosController(ApplicationDbContext context
            , RT.Comb.ICombProvider comb
            ) : base(context, comb)
        {
        }

        // GET: Portal/Horarios
        public async Task<IActionResult> Index()
        {

            var userId = Guid.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userHorarios = _context.Horarios
                .Include(h => h.Disciplina)
                .Where(h => h.UsuarioId == userId);

            return View(await userHorarios.ToListAsync());
        }

        // GET: Portal/Horarios/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horario = await _context.Horarios
                .Include(h => h.Disciplina)
                .Include(h => h.Usuario)
                .FirstOrDefaultAsync(m => m.HorarioId == id);
            if (horario == null)
            {
                return NotFound();
            }

            return View(horario);
        }

        // GET: Portal/Horarios/Create
        public IActionResult Create()
        {
            var disciplinas = _context.Disciplinas.ToList();

            if (disciplinas.Count == 0)
            {
                this.MostrarMensagem($"Não há disciplinas cadastradas. Por favor, cadastre uma disciplina antes de adicionar horários.", erro: true);
                return RedirectToAction(nameof(Index));
            }

            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "DisciplinaId", "Nome");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeCompleto");
            return View();
        }

        // POST: Portal/Horarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HorarioId,DisciplinaId,DataAula,DataCadastro")] Horario horario)
        {
            var userId = Guid.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (ModelState.IsValid)
            {
                horario.HorarioId = _comb.Create();
                horario.UsuarioId = userId;

                _context.Add(horario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "DisciplinaId", "Nome", horario.DisciplinaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeCompleto", horario.UsuarioId);
            return View(horario);
        }

        // GET: Portal/Horarios/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horario = await _context.Horarios.FindAsync(id);
            if (horario == null)
            {
                return NotFound();
            }
            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "DisciplinaId", "Nome", horario.DisciplinaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeCompleto", horario.UsuarioId);
            return View(horario);
        }

        // POST: Portal/Horarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("HorarioId,UsuarioId,DisciplinaId,DataAula,DataCadastro")] Horario horario)
        {
            if (id != horario.HorarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(horario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HorarioExists(horario.HorarioId))
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
            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "DisciplinaId", "Nome", horario.DisciplinaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeCompleto", horario.UsuarioId);
            return View(horario);
        }

        // GET: Portal/Horarios/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horario = await _context.Horarios
                .Include(h => h.Disciplina)
                .Include(h => h.Usuario)
                .FirstOrDefaultAsync(m => m.HorarioId == id);
            if (horario == null)
            {
                return NotFound();
            }

            return View(horario);
        }

        // POST: Portal/Horarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var horario = await _context.Horarios.FindAsync(id);
            if (horario != null)
            {
                _context.Horarios.Remove(horario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HorarioExists(Guid id)
        {
            return _context.Horarios.Any(e => e.HorarioId == id);
        }
    }
}