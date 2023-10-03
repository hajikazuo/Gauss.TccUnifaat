using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gauss.TccUnifaat.Data;
using Gauss.TccUnifaat.Models;
using System.Security.Claims;

namespace Gauss.TccUnifaat.Controllers
{
    public class NoticiasController : ControllerBase
    {
        private string _filePath;

        public NoticiasController(ApplicationDbContext context, IWebHostEnvironment env
            , RT.Comb.ICombProvider comb) : base(context, comb)
        {
            _filePath = env.WebRootPath;
        }

        // GET: Noticias
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Noticias.Include(n => n.Categoria).Include(n => n.Usuario);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Noticias/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Noticias == null)
            {
                return NotFound();
            }

            var noticia = await _context.Noticias
                .Include(n => n.Categoria)
                .Include(n => n.Usuario)
                .FirstOrDefaultAsync(m => m.NoticiaId == id);
            if (noticia == null)
            {
                return NotFound();
            }

            return View(noticia);
        }

        public bool ValidaImagem(IFormFile anexo)
        {
            switch (anexo.ContentType)
            {
                case "image/jpeg":
                    return true;
                case "image/bmp":
                    return true;
                case "image/gif":
                    return true;
                case "image/png":
                    return true;
                default:
                    return false;
            }
        }

        public string SalvarArquivo(IFormFile anexo)
        {
            var nome = Guid.NewGuid().ToString() + anexo.FileName;

            var filePath = _filePath + "\\fotos";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            using (var stream = System.IO.File.Create(filePath + "\\" + nome))
            {
                anexo.CopyToAsync(stream);
            }

            return nome;
        }

        // GET: Noticias/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaNome");
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Noticias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Noticia noticia, IFormFile anexo)
        {
            var userId = Guid.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (ModelState.IsValid)
            {
                if (!ValidaImagem(anexo))
                    return View(noticia);

                var nome = SalvarArquivo(anexo);
                noticia.Foto = nome;
                noticia.NoticiaId = _comb.Create();
                noticia.UsuarioId = userId;
                noticia.TipoNoticia = Models.Enums.TipoNoticia.NoticiaPrincipal;
                _context.Add(noticia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaNome", noticia.CategoriaId);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id", noticia.UsuarioId);
            return View(noticia);
        }

        // GET: Noticias/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Noticias == null)
            {
                return NotFound();
            }

            var noticia = await _context.Noticias.FindAsync(id);
            if (noticia == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaNome", noticia.CategoriaId);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id", noticia.UsuarioId);
            return View(noticia);
        }

        // POST: Noticias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("NoticiaId,UsuarioId,CategoriaId,TipoNoticia,Titulo,Conteudo,DataCadastro,Foto")] Noticia noticia)
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
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaNome", noticia.CategoriaId);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id", noticia.UsuarioId);
            return View(noticia);
        }

        // GET: Noticias/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Noticias == null)
            {
                return NotFound();
            }

            var noticia = await _context.Noticias
                .Include(n => n.Categoria)
                .Include(n => n.Usuario)
                .FirstOrDefaultAsync(m => m.NoticiaId == id);
            if (noticia == null)
            {
                return NotFound();
            }

            return View(noticia);
        }

        // POST: Noticias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Noticias == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Noticias'  is null.");
            }
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
          return (_context.Noticias?.Any(e => e.NoticiaId == id)).GetValueOrDefault();
        }
    }
}
