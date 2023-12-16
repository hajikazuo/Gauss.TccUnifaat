using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Gauss.TccUnifaat.Common.Models
{
    public class Usuario : IdentityUser<Guid>
    {
        [MaxLength(128)]
        public string NomeCompleto { get; set; }

        [MaxLength(20)]
        public string Cpf { get; set; }

        [MaxLength(20)]
        public string Telefone { get; set; }

        public int Idade { get; set; }

        [JsonIgnore]
        public virtual ICollection<Noticia>? Noticias { get; set; }
    }
}
