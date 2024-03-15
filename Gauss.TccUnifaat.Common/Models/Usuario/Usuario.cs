using Gauss.TccUnifaat.Common.Resources;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Gauss.TccUnifaat.Common.Models
{
    public class Usuario : IdentityUser<Guid>
    {
        [MaxLength(128, ErrorMessageResourceType = typeof(TextosValidacao), ErrorMessageResourceName = nameof(TextosValidacao.MaxLength))]
        [Display(Name = "Nome completo")]
        public string NomeCompleto { get; set; }

        [MaxLength(20)]
        [Display(Name = "CPF")]
        public string? Cpf { get; set; }

        [MaxLength(20, ErrorMessageResourceType = typeof(TextosValidacao), ErrorMessageResourceName = nameof(TextosValidacao.MaxLength))]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; }

        [Required(ErrorMessageResourceType = typeof(TextosValidacao), ErrorMessageResourceName = nameof(TextosValidacao.Required))]
        [Display(Name = "Idade")]
        public int Idade { get; set; }

        [JsonIgnore]
        public virtual ICollection<Noticia>? Noticias { get; set; }

        [JsonIgnore]
        public virtual ICollection<Presenca>? Presencas { get; set; }

        public Guid? TurmaId { get; set; }

        [JsonIgnore]
        public virtual Turma? Turma { get; set; }
    }
}

