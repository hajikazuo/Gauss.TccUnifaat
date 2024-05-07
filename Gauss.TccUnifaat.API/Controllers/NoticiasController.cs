using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Common.Services.Interfaces;
using Gauss.TccUnifaat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gauss.TccUnifaat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticiasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NoticiasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Noticias1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Noticia>>> GetNoticia()
        {
            if (_context.Noticias == null)
            {
                return NotFound();
            }
            return await _context.Noticias.ToListAsync();
        }

        // GET: api/Noticias1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Noticia>> GetNoticia(Guid id)
        {
            if (_context.Noticias == null)
            {
                return NotFound();
            }
            var noticia = await _context.Noticias.FindAsync(id);

            if (noticia == null)
            {
                return NotFound();
            }

            return noticia;
        }

        // PUT: api/Noticias1/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNoticia(Guid id, Noticia noticia)
        {
            if (id != noticia.NoticiaId)
            {
                return BadRequest();
            }

            _context.Entry(noticia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoticiaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Noticias1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Noticia>> PostNoticia(Noticia noticia)
        {
            if (_context.Noticias == null)
            {
                return Problem("Entity set 'GaussTccUnifaatContext.Noticia'  is null.");
            }
            _context.Noticias.Add(noticia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNoticia", new { id = noticia.NoticiaId }, noticia);
        }

        // DELETE: api/Noticias1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNoticia(Guid id)
        {
            if (_context.Noticias == null)
            {
                return NotFound();
            }
            var noticia = await _context.Noticias.FindAsync(id);
            if (noticia == null)
            {
                return NotFound();
            }

            _context.Noticias.Remove(noticia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NoticiaExists(Guid id)
        {
            return (_context.Noticias?.Any(e => e.NoticiaId == id)).GetValueOrDefault();
        }
    }
}
