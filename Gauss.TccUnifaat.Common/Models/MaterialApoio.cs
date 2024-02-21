using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gauss.TccUnifaat.Common.Models
{
    public class MaterialApoio
    {
        public Guid MaterialApoioId { get; set; }

        [MaxLength(100)]
        [Display(Name = "Nome do Material/Atividade")]
        public string Nome { get; set; }

        [MaxLength(500)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [MaxLength(250)]
        public string? Arquivo { get; set; }

        [Display(Name = "Disciplina")]
        public Guid DisciplinaId { get; set; }
        public virtual Disciplina? Disciplina { get; set; }
    }
}
