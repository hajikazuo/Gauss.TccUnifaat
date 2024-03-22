using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gauss.TccUnifaat.Common.Models
{
    public class Horario : IStatusModificacao
    {
        public Guid HorarioId { get; set; }

        [Display(Name = "Professor")]
        public Guid UsuarioId { get; set; }

        [Display(Name = "Disciplina")]
        public Guid DisciplinaId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data da aula")]
        public DateTime DataAula { get; set; }

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

        [ForeignKey(nameof(UsuarioId))]
        public virtual Usuario? Usuario { get; set; }
        public virtual Disciplina? Disciplina { get; set; }
    }
}
