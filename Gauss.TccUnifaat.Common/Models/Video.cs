using Gauss.TccUnifaat.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace Gauss.TccUnifaat.Common.Models;

public class Video : IStatusModificacao 
{
    public Guid VideoId { get; set; }

    [Required(ErrorMessageResourceType = typeof(TextosValidacao), ErrorMessageResourceName = nameof(TextosValidacao.Required))]
    [Display(Name = "Título")]
    public string Titulo { get; set; }

    [Required(ErrorMessageResourceType = typeof(TextosValidacao), ErrorMessageResourceName = nameof(TextosValidacao.Required))]
    [Display(Name = "Link do YouTube")]
    public string LinkYouTube { get; set; }

    [Display(Name = "Disciplina")]
    public Guid DisciplinaId { get; set; }
    public Disciplina? Disciplina { get; set; }

    #region Interface
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    [DataType(DataType.DateTime)]
    [Display(Name = "Data Cadastro")]
    public DateTime DataCadastro { get; set; }

    [ScaffoldColumn(false)]
    [Display(Name = "Excluído")]
    public bool Excluido { get; set; }

    [ScaffoldColumn(false)]
    [Display(Name = "Data Excluído")]
    public DateTime? DataExcluido { get; set; }

    [ScaffoldColumn(false)]
    [Display(Name = "Data Últ. Modificação")]
    public DateTime? DataUltimaModificacao { get; set; }
    #endregion
}
