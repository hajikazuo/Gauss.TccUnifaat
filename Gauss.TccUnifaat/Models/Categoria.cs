using Gauss.TccUnifaat.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Gauss.TccUnifaat.Models
{
    public class Categoria : IStatusModificacao
    {
        public Guid CategoriaId { get; set; }

        [StringLength(100, ErrorMessage = "O tamanho máximo é 100 caracteres")]
        [Required(ErrorMessage = "Informe o nome da categoria")]
        [Display(Name = "Nome")]
        public string CategoriaNome { get; set; }

        [StringLength(200, ErrorMessage = "O tamanho máximo é 200 caracteres")]
        [Required(ErrorMessage = "Informe a descrição da categoria")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

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

        public List<Noticia>? Noticias { get; set; }
    }
}
