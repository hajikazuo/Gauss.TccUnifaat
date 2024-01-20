using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gauss.TccUnifaat.Common.Models
{
    public class Presenca : IStatusModificacao
    {
        public Guid Id { get; set; }
        public DateTime DataAula { get; set; }
        public bool Presente { get; set; }

        public Guid TurmaId { get; set; }
        public virtual Turma? Turma { get; set; }

        public Guid UsuarioId { get; set; }
        public virtual Usuario? Usuario { get; set; }

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
