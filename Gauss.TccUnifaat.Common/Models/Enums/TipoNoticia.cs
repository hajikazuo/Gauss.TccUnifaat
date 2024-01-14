using System.ComponentModel.DataAnnotations;

namespace Gauss.TccUnifaat.Common.Models
{
    public enum TipoNoticia
    {
        [Display(Name = "Nenhum")]
        Nenhuma =0,

        [Display(Name = "Notícia da página principal")]
        NoticiaPrincipal =1,

        [Display(Name = "Informativos")]
        Informativo = 2,

        [Display(Name = "Relatos")]
        Relato = 3,

        [Display(Name = "Notícia do Cursinho")]
        Cursinho = 4,

        [Display(Name = "Notícia do curso de Programação")]
        Programacao = 5,

    }
}
