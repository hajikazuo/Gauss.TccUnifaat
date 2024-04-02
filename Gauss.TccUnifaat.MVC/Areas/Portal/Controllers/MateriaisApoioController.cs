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
    public class MateriaisApoioController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly string _filePath;
        private readonly IWebHostEnvironment _env;
        public MateriaisApoioController(ApplicationDbContext context
            , RT.Comb.ICombProvider comb, UserManager<Usuario> userManager, IWebHostEnvironment env
            ) : base(context, comb)
        {
            _userManager = userManager;
            _env = env;
            _filePath = Path.Combine(_env.WebRootPath, "files");
            if (!Directory.Exists(_filePath))
            {
                Directory.CreateDirectory(_filePath);
            }
        }

        // GET: Portal/MateriaisApoio
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var materiaisApoioDaTurma = await _context.MateriaisApoio
           .Where(m => m.Disciplina.TurmaId == currentUser.TurmaId)
           .Include(m => m.Disciplina)
           .ToListAsync();

            return View(materiaisApoioDaTurma);
        }

        // GET: Portal/MateriaisApoio/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialApoio = await _context.MateriaisApoio
                .Include(m => m.Disciplina)
                .FirstOrDefaultAsync(m => m.MaterialApoioId == id);
            if (materialApoio == null)
            {
                return NotFound();
            }

            return View(materialApoio);
        }

        // GET: Portal/MateriaisApoio/Create
        public IActionResult Create()
        {
            var disciplinas = _context.Disciplinas.ToList();

            if (disciplinas.Count == 0)
            {
                TempData["Message"] = "Não há disciplinas cadastradas. Por favor, cadastre uma disciplina antes de adicionar materiais de apoio.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "DisciplinaId", "Nome");
            return View();
        }

        // POST: Portal/MateriaisApoio/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaterialApoioId,Nome,Descricao,Arquivo,DisciplinaId")] MaterialApoio materialApoio, IFormFile anexo)
        {
            if (ModelState.IsValid)
            {
                if (anexo != null && anexo.Length > 0)
                {
                    materialApoio.Arquivo = await SalvarArquivo(anexo);
                }

                materialApoio.MaterialApoioId = _comb.Create();
                _context.Add(materialApoio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "DisciplinaId", "Nome", materialApoio.DisciplinaId);
            return View(materialApoio);
        }

        // GET: Portal/MateriaisApoio/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialApoio = await _context.MateriaisApoio.FindAsync(id);
            if (materialApoio == null)
            {
                return NotFound();
            }
            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "DisciplinaId", "Nome", materialApoio.DisciplinaId);
            return View(materialApoio);
        }

        // POST: Portal/MateriaisApoio/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("MaterialApoioId,Nome,Descricao,Arquivo,DisciplinaId")] MaterialApoio materialApoio, IFormFile arquivo)
        {
            if (id != materialApoio.MaterialApoioId)
            {
                return NotFound();
            }

            try
            {
                var materialApoioExistente = await _context.MateriaisApoio.FindAsync(id);

                if (materialApoioExistente != null)
                {
                    if (arquivo != null && arquivo.Length > 0)
                    {
                        materialApoio.Arquivo = await SalvarArquivo(arquivo);
                    }
                    else
                    {
                        materialApoio.Arquivo = materialApoioExistente.Arquivo;
                    }

                    _context.Entry(materialApoioExistente).CurrentValues.SetValues(materialApoio);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialApoioExists(materialApoio.MaterialApoioId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // GET: Portal/MateriaisApoio/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialApoio = await _context.MateriaisApoio
                .Include(m => m.Disciplina)
                .FirstOrDefaultAsync(m => m.MaterialApoioId == id);
            if (materialApoio == null)
            {
                return NotFound();
            }

            return View(materialApoio);
        }

        // POST: Portal/MateriaisApoio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var materialApoio = await _context.MateriaisApoio.FindAsync(id);
            if (materialApoio != null)
            {
                _context.MateriaisApoio.Remove(materialApoio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialApoioExists(Guid id)
        {
            return _context.MateriaisApoio.Any(e => e.MaterialApoioId == id);
        }

        private async Task<string> SalvarArquivo(IFormFile anexo)
        {
            if (anexo != null && anexo.Length > 0)
            {
                var nome = Guid.NewGuid().ToString() + Path.GetExtension(anexo.FileName);
                var filePath = Path.Combine(_filePath, nome);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await anexo.CopyToAsync(stream);
                }

                return nome;
            }
            return null;
        }
    }
}
