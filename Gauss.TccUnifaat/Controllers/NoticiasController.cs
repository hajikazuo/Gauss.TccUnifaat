using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Gauss.TccUnifaat.Data;
using Gauss.TccUnifaat.Models;
using System.Security.Claims;
using Gauss.TccUnifaat.Models.Enums;

namespace Gauss.TccUnifaat.Controllers
{
    public class NoticiasController : ControllerBase
    {
        private readonly string _filePath;
        private readonly IWebHostEnvironment _env;

        public NoticiasController(ApplicationDbContext context, IWebHostEnvironment env, RT.Comb.ICombProvider comb) : base(context, comb)
        {
            _filePath = Path.Combine(env.WebRootPath, "img");
            _env = env;
        }

        // GET: Noticias
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Noticias.Include(n => n.Usuario);
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
            if (anexo != null)
            {
                var allowedTypes = new[] { "image/jpeg", "image/bmp", "image/gif", "image/png" };
                return allowedTypes.Contains(anexo.ContentType);
            }
            return false;
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

        // GET: Noticias/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id");

            var tipoNoticiaOptions = Enum.GetValues(typeof(TipoNoticia))
                .Cast<TipoNoticia>()
                .Select(e => new SelectListItem
                {
                    Text = e.ToString(),
                    Value = ((int)e).ToString()
                });
            ViewData["TipoNoticiaOptions"] = tipoNoticiaOptions;

            return View();
        }

        // POST: Noticias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Noticia noticia, IFormFile anexo)
        {
            var userId = Guid.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (ModelState.IsValid)
            {
                if (!ValidaImagem(anexo))
                {
                    ModelState.AddModelError("Foto", "Formato de imagem inválido. Use .jpg, .bmp, .gif ou .png.");
                    return View(noticia);
                }

                var nome = SalvarArquivo(anexo);
                if (nome != null)
                {
                    noticia.Foto = nome;
                    noticia.NoticiaId = _comb.Create();
                    noticia.UsuarioId = userId;

                // Converta o valor selecionado de volta para o enum TipoNoticia
                noticia.TipoNoticia = (TipoNoticia)Enum.Parse(typeof(TipoNoticia), noticia.TipoNoticia.ToString());

                    _context.Add(noticia);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id", noticia.UsuarioId);

            var tipoNoticiaOptions = Enum.GetValues(typeof(TipoNoticia))
                .Cast<TipoNoticia>()
                .Select(e => new SelectListItem
                {
                    Text = e.ToString(),
                    Value = ((int)e).ToString()
                });
            ViewData["TipoNoticiaOptions"] = tipoNoticiaOptions;

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
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id", noticia.UsuarioId);

            var tipoNoticiaOptions = Enum.GetValues(typeof(TipoNoticia))
                .Cast<TipoNoticia>()
                .Select(e => new SelectListItem
                {
                    Text = e.ToString(),
                    Value = ((int)e).ToString()
                });
            ViewData["TipoNoticiaOptions"] = tipoNoticiaOptions;

            return View(noticia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Noticia noticia)
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
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id", noticia.UsuarioId);

            var tipoNoticiaOptions = Enum.GetValues(typeof(TipoNoticia))
                .Cast<TipoNoticia>()
                .Select(e => new SelectListItem
                {
                    Text = e.ToString(),
                    Value = ((int)e).ToString()
                });
            ViewData["TipoNoticiaOptions"] = tipoNoticiaOptions;

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
            var noticia = await _context.Noticias.FindAsync(id);
            if (noticia != null)
            {
                _context.Noticias.Remove(noticia);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool NoticiaExists(Guid id)
        {
            return (_context.Noticias?.Any(e => e.NoticiaId == id)).GetValueOrDefault();
        }


    }
}



