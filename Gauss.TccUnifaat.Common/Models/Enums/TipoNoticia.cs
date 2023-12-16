using System.ComponentModel.DataAnnotations;

namespace Gauss.TccUnifaat.Common.Models
{
    public enum TipoNoticia
    {
        [Display(Name = "Nenhum")]
        Nenhuma =0,
        [Display(Name = "Notícias principais")]
        NoticiaPrincipal =1,
        [Display(Name = "Informativos")]
        Informativo = 2,
        [Display(Name = "Relatos")]
        Relato = 3
    }
}
