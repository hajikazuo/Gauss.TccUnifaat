using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gauss.TccUnifaat.Common.Models
{
    public class Turma : IStatusModificacao
    {
        public Guid TurmaId { get; set; }

        [MaxLength(50)]
        public string Nome { get; set; }

        public virtual ICollection<Usuario>? Usuarios { get; set; } = new List<Usuario>();
        public virtual ICollection<Disciplina>? Disciplinas { get; set; } = new List<Disciplina>();
        public virtual ICollection<Presenca>? Presencas { get; set; } = new List<Presenca>();
        public virtual ICollection<Aviso>? Avisos { get; set; } = new List<Aviso>();

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
