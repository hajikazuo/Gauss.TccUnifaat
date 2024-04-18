using Gauss.TccUnifaat.Common.Models;

namespace Gauss.TccUnifaat.MVC.Services.Interfaces
{
    public interface INoticiaService
    {
        Task<List<Noticia>> ObterNoticiasAsync(string query, DateTime dataInicial);
        Task SalvarNoticiasAsync(List<Noticia> noticias);
    }
}

