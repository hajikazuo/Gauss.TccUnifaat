using Gauss.TccUnifaat.Common.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gauss.TccUnifaat.Common.Models
{
    public class Disciplina : IStatusModificacao
    {
        public Guid DisciplinaId { get; set; }

        [Required(ErrorMessageResourceType = typeof(TextosValidacao), ErrorMessageResourceName = nameof(TextosValidacao.Required))]
        [Display(Name = "Nome da Disciplina")]
        [MaxLength(50, ErrorMessageResourceType = typeof(TextosValidacao), ErrorMessageResourceName = nameof(TextosValidacao.MaxLength))]
        public string Nome { get; set; }


        [Display(Name = "Turma")]
        public Guid TurmaId { get; set; }
        public virtual Turma? Turma { get; set; }

        public virtual ICollection<MaterialApoio>? MateriaisApoio { get; set; } = new List<MaterialApoio>();

        public virtual ICollection<Video> Videos { get; set; } = new List<Video>();

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
}
