﻿using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.MVC.Services.Interfaces;
using NewsAPI.Constants;
using NewsAPI.Models;
using NewsAPI;
using Gauss.TccUnifaat.MVC.Settings;
using Microsoft.Extensions.Options;
using Gauss.TccUnifaat.Data;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Noticia>> ObterNoticiasAsync(string query, DateTime dataInicial)
        {
            var response = await _newsApiClient.GetEverythingAsync(new EverythingRequest
            {
                Q = query,
                SortBy = SortBys.Popularity,
                From = dataInicial
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
