using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Data;
using Gauss.TccUnifaat.Controllers;
using Gauss.TccUnifaat.MVC.Extensions;

namespace Gauss.TccUnifaat.MVC.Areas.Portal.Controllers
{
    [Area("Portal")]
    public class VideosController : ControllerBase<ApplicationDbContext, RT.Comb.ICombProvider>
    {
        public VideosController(ApplicationDbContext context
            , RT.Comb.ICombProvider comb
            ) : base(context, comb)
        {
        }

        // GET: Portal/Videos
        public async Task<IActionResult> Index()
        {
            var videosPorDisciplina = await _context.Videos
                .Include(v => v.Disciplina)
                .GroupBy(v => v.Disciplina.Nome) 
                .ToListAsync();

            return View(videosPorDisciplina);
        }


        // GET: Portal/Videos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Videos
                .Include(v => v.Disciplina)
                .FirstOrDefaultAsync(m => m.VideoId == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // GET: Portal/Videos/Create
        public IActionResult Create()
        {
            var disciplinas = _context.Disciplinas.ToList();

            if (disciplinas.Count == 0)
            {
                this.MostrarMensagem($"Não há disciplinas cadastradas. Por favor, cadastre uma disciplina antes de adicionar videos.", erro: true);
                return RedirectToAction(nameof(Index));
            }

            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "DisciplinaId", "Nome");
            return View();
        }

        // POST: Portal/Videos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VideoId,Titulo,LinkYouTube,DisciplinaId")] Video video)
        {
            video.LinkYouTube = ExtractYouTubeVideoId(video.LinkYouTube);

            if (ModelState.IsValid)
            {
                video.VideoId = _comb.Create();
                _context.Add(video);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "DisciplinaId", "Nome", video.DisciplinaId);
            return View(video);
        }

        // GET: Portal/Videos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Videos.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }
            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "DisciplinaId", "Nome", video.DisciplinaId);
            return View(video);
        }

        // POST: Portal/Videos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("VideoId,Titulo,LinkYouTube,DisciplinaId,DataCadastro")] Video video)
        {
            if (id != video.VideoId)
            {
                return NotFound();
            }

            video.LinkYouTube = ExtractYouTubeVideoId(video.LinkYouTube);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(video);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoExists(video.VideoId))
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
            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "DisciplinaId", "Nome", video.DisciplinaId);
            return View(video);
        }

        // GET: Portal/Videos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Videos
                .Include(v => v.Disciplina)
                .FirstOrDefaultAsync(m => m.VideoId == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // POST: Portal/Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video != null)
            {
                _context.Videos.Remove(video);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoExists(Guid id)
        {
            return _context.Videos.Any(e => e.VideoId == id);
        }

        private string ExtractYouTubeVideoId(string youtubeLink)
        {
            try
            {
                string[] parts = youtubeLink.Split('=');

                string videoId = parts[parts.Length - 1];

                int ampersandIndex = videoId.IndexOf('&');
                if (ampersandIndex != -1)
                {
                    videoId = videoId.Substring(0, ampersandIndex);
                }

                return videoId;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
