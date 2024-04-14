using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Data;
using Gauss.TccUnifaat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gauss.TccUnifaat.Controllers
{
    [AllowAnonymous]
    public class HomeController : ControllerBase<ApplicationDbContext, RT.Comb.ICombProvider>
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Usuario> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<Usuario> userManager, RT.Comb.ICombProvider comb, ApplicationDbContext context)
        : base(context, comb)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var noticiasDaCamadaDeDados = _context.Noticias.ToList();

            var noticiasViewModel = noticiasDaCamadaDeDados
                .OrderByDescending(noticia => noticia.DataCadastro)
                .Take(3)
                .Select(noticia => new NoticiasViewModel
            {
                Titulo = noticia.Titulo,
                Conteudo = noticia.Conteudo,
                DataCadastro = noticia.DataCadastro,
                UrlImagem = Url.Content($"~/imgNoticias/{noticia.Foto}"),
            }).ToList();

            return View(noticiasViewModel);
        }

        public IActionResult Sobre()
        {
            return View();
        }

        public IActionResult Contato()
        {
            return View();
        }


        public IActionResult Cursinho()
        {
            var noticiasDaCamadaDeDados = _context.Noticias.ToList();

            var noticiasViewModel = noticiasDaCamadaDeDados
                .Where(n => n.TipoNoticia == TipoNoticia.Cursinho)
                .Select(noticia => new NoticiasViewModel
                {
                    Titulo = noticia.Titulo,
                    Conteudo = noticia.Conteudo,
                    DataCadastro = noticia.DataCadastro,
                    UrlImagem = Url.Content($"~/imgNoticias/{noticia.Foto}"),
                }).ToList();

            return View(noticiasViewModel);
        }

        public IActionResult Programacao()
        {
            var noticiasDaCamadaDeDados = _context.Noticias.ToList();

            var noticiasViewModel = noticiasDaCamadaDeDados
                .Where(n => n.TipoNoticia == TipoNoticia.Programacao)
                .Select(noticia => new NoticiasViewModel
                {
                    Titulo = noticia.Titulo,
                    Conteudo = noticia.Conteudo,
                    DataCadastro = noticia.DataCadastro,
                    UrlImagem = Url.Content($"~/imgNoticias/{noticia.Foto}"),
                }).ToList();

            return View(noticiasViewModel);
        }
    }
}