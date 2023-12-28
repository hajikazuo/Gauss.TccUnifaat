using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gauss.TccUnifaat.Common.Models
{
    public class Turma 
    {
        public Guid TurmaId { get; set; }

        [MaxLength(50)]
        public string Nome { get; set; }

    }
}
