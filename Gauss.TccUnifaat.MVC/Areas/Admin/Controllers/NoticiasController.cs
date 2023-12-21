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
using Gauss.TccUnifaat.Common.Extensions;

namespace Gauss.TccUnifaat.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NoticiasController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RT.Comb.ICombProvider _comb;
        private readonly string _filePath;
        private readonly IWebHostEnvironment _env;

        public NoticiasController(ApplicationDbContext context, RT.Comb.ICombProvider comb, IWebHostEnvironment env)
        {
            _filePath = Path.Combine(env.WebRootPath, "img");
            _context = context;
            _comb = comb;
        }

        // GET: Admin/Noticias
        public async Task<IActionResult> Index()
        {
            var gaussTccUnifaatMVCContext = _context.Noticias.Include(n => n.Usuario);
            return View(await gaussTccUnifaatMVCContext.ToListAsync());
        }

        // GET: Admin/Noticias/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noticia = await _context.Noticias
                .Include(n => n.Usuario)
                .FirstOrDefaultAsync(m => m.NoticiaId == id);
            if (noticia == null)
            {
                return NotFound();
            }

            return View(noticia);
        }

        public string SalvarArquivo(IFormFile anexo)
        {


            if (anexo != null && anexo.Length > 0)
            {
                var nome = Guid.NewGuid().ToString() + Path.GetExtension(anexo.FileName);
                var filePath = Path.Combine(_filePath, nome);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    anexo.CopyTo(stream);
                }

                return nome;
            }
            return null;
        }
        // GET: Admin/Noticias/Create
        public IActionResult Create()
        {
            var teste = new Noticia();
            ViewData["TipoNoticia"] = ControllerEnumsExtensions.MontarSelectListParaEnum2(teste.TipoNoticia, true);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Noticia noticia, IFormFile anexo)
        {
            var userId = Guid.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (ModelState.IsValid)
            {
                noticia.NoticiaId = _comb.Create();
                noticia.UsuarioId = userId;

                noticia.Foto = SalvarArquivo(anexo);

                _context.Noticias.Add(noticia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TipoNoticia"] = ControllerEnumsExtensions.MontarSelectListParaEnum2(noticia.TipoNoticia, true);

            return View(noticia);
        }


        // GET: Admin/Noticias/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noticia = await _context.Noticias.FindAsync(id);
            if (noticia == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Set<Usuario>(), "Id", "Cpf", noticia.UsuarioId);
            return View(noticia);
        }

        // POST: Admin/Noticias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("NoticiaId,UsuarioId,TipoNoticia,Titulo,Conteudo,DataCadastro,Foto")] Noticia noticia)
        {
            if (id != noticia.NoticiaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(noticia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoticiaExists(noticia.NoticiaId))
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
            ViewData["UsuarioId"] = new SelectList(_context.Set<Usuario>(), "Id", "Cpf", noticia.UsuarioId);
            return View(noticia);
        }

        // GET: Admin/Noticias/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noticia = await _context.Noticias
                .Include(n => n.Usuario)
                .FirstOrDefaultAsync(m => m.NoticiaId == id);
            if (noticia == null)
            {
                return NotFound();
            }

            return View(noticia);
        }

        // POST: Admin/Noticias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var noticia = await _context.Noticias.FindAsync(id);
            if (noticia != null)
            {
                _context.Noticias.Remove(noticia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoticiaExists(Guid id)
        {
            return _context.Noticias.Any(e => e.NoticiaId == id);
        }
    }
}
