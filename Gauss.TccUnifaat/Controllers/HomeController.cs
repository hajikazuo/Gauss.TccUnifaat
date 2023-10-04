using Gauss.TccUnifaat.Data;
using Gauss.TccUnifaat.Models;
using Gauss.TccUnifaat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Gauss.TccUnifaat.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Usuario> _userManager;
        public RT.Comb.ICombProvider _comb;

        public HomeController(ILogger<HomeController> logger, UserManager<Usuario> userManager, RT.Comb.ICombProvider comb, ApplicationDbContext context)
        : base(context, comb)
        {
            _logger = logger;
            _userManager = userManager;
            _comb = comb;
        }

        public IActionResult Index()
        {

            var noticiasDaCamadaDeDados = _context.Noticias.ToList();

            var noticiasViewModel = noticiasDaCamadaDeDados.Select(noticia => new HomePageViewModel
            {
                Titulo = noticia.Titulo,
                Conteudo = noticia.Conteudo,
                DataCadastro = noticia.DataCadastro,
                UrlImagem = Url.Content($"~/fotos/{noticia.Foto}"),
            }).ToList();

            return View(noticiasViewModel);
        }

    }
}