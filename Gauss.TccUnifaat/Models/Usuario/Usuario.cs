using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Gauss.TccUnifaat.Models
{
    public class Usuario : IdentityUser<Guid>
    {
        [MaxLength(128)]
        public string NomeCompleto { get; set; }

        [JsonIgnore]
        public virtual ICollection<Noticia>? Noticias { get; set; }
    }
}
