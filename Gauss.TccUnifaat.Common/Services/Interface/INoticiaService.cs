using Gauss.TccUnifaat.Common.Models;

namespace Gauss.TccUnifaat.Common.Services.Interfaces
{
    public interface INoticiaService
    {
        Task<List<Noticia>> ObterNoticiasAsync();
        Task SalvarNoticiasAsync(List<Noticia> noticias);
    }
}

