using Gauss.TccUnifaat.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace Gauss.TccUnifaat.ViewModels
{
    public class LoginViewModel
    {     
        [Display(Name = "Usuário")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessageResourceType = typeof(TextosValidacao), ErrorMessageResourceName = nameof(TextosValidacao.Required))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(TextosValidacao), ErrorMessageResourceName = nameof(TextosValidacao.Required))]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }
    }
}
