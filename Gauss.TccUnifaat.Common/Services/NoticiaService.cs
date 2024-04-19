using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Common.Services.Interfaces;
using Gauss.TccUnifaat.Common.Settings;
using Gauss.TccUnifaat.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;

namespace Gauss.TccUnifaat.MVC.Services
{
    public class NoticiaService : INoticiaService
    {
        private readonly NewsApiClient _newsApiClient;
        private readonly string _apiKey;
        private readonly ApplicationDbContext _context;

        public NoticiaService(IOptions<NewsApiSettings> apiSettings, ApplicationDbContext context)
        {
            _apiKey = apiSettings.Value.ChaveApi;
            _newsApiClient = new NewsApiClient(_apiKey);
            _context = context;
        }

        public async Task<List<Noticia>> ObterNoticiasAsync()
        {
            DateTime dataNoticia = DateTime.Today.AddDays(-1);

            var response = await _newsApiClient.GetEverythingAsync(new EverythingRequest
            {
                Q = "educação",
                SortBy = SortBys.Popularity,
                From = dataNoticia
            });

            if (response.Status == Statuses.Ok)
            {
                var noticias = response.Articles
                .Take(3)
                .Select(item => new Noticia
                {
                    NoticiaId = Guid.NewGuid(),
                    TipoNoticia = TipoNoticia.NoticiaAPI,
                    Titulo = item.Title,
                    Conteudo = item.Content,
                    Foto = item.UrlToImage,
                    Link = item.Url,
                }).ToList();

                await SalvarNoticiasAsync(noticias);

                return noticias;
            }
            else
            {
                throw new Exception("Erro ao obter os dados da API.");
            }
        }

        public async Task SalvarNoticiasAsync(List<Noticia> noticias)
        {
            foreach (var noticia in noticias)
            {
                var existe = await _context.Noticias.FirstOrDefaultAsync(n => n.Titulo == noticia.Titulo);

                if (existe == null)
                {
                    _context.Noticias.Add(noticia);
                }
            }

            await _context.SaveChangesAsync();
        }

    }
}
