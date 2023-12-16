using System.ComponentModel.DataAnnotations;

namespace Gauss.TccUnifaat.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O campo {0} é requerido!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "O campo {0} é requerido!")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }
    }
}
