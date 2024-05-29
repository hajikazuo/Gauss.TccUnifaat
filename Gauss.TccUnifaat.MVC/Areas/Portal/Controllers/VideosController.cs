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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Gauss.TccUnifaat.MVC.Areas.Portal.Controllers
{
    [Area("Portal")]
    public class VideosController : ControllerBase<ApplicationDbContext, RT.Comb.ICombProvider>
    {
        private readonly UserManager<Usuario> _userManager;
        public VideosController(ApplicationDbContext context
            , RT.Comb.ICombProvider comb, UserManager<Usuario> userManager
            ) : base(context, comb)
        {
            _userManager = userManager;
        }

        // GET: Portal/Videos
        public async Task<IActionResult> Index()
        {

            var currentUser = await _userManager.GetUserAsync(User);

            var videosPorDisciplina = await _context.Videos
                .Where(v => v.Disciplina.TurmaId == currentUser.TurmaId)
                .Include(v => v.Disciplina)
                .GroupBy(v => v.Disciplina.Nome)
                .ToListAsync();

            return View(videosPorDisciplina);
        }

        [Authorize(Policy = "RequireAdminOrProfessorRole")]
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

        [Authorize(Policy = "RequireAdminOrProfessorRole")]
        // GET: Portal/Videos/Create
        public async Task<IActionResult> CreateAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            
            if (currentUser.TurmaId == null)
            {
                this.MostrarMensagem($"Você não está associado a uma turma. Por favor, entre em contato com o administrador do sistema.", erro: true);
                return RedirectToAction(nameof(Index));
            }

            var disciplinas = _context.Disciplinas.Where(v => v.TurmaId == currentUser.TurmaId).ToList();

            if (disciplinas.Count == 0)
            {
                this.MostrarMensagem($"Não há disciplinas cadastradas. Por favor, cadastre uma disciplina antes de adicionar videos.", erro: true);
                return RedirectToAction(nameof(Index));
            }

            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas.Where(v => v.TurmaId == currentUser.TurmaId), "DisciplinaId", "Nome");
            return View();
        }

        [Authorize(Policy = "RequireAdminOrProfessorRole")]
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

        [Authorize(Policy = "RequireAdminOrProfessorRole")]
        // GET: Portal/Videos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var video = await _context.Videos.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }
            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas.Where(v => v.TurmaId == currentUser.TurmaId), "DisciplinaId", "Nome", video.DisciplinaId);
            return View(video);
        }

        [Authorize(Policy = "RequireAdminOrProfessorRole")]
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

        [Authorize(Policy = "RequireAdminOrProfessorRole")]
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

        [Authorize(Policy = "RequireAdminOrProfessorRole")]
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
