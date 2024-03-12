using System.ComponentModel.DataAnnotations;

namespace Gauss.TccUnifaat.MVC.ViewModels
{
    public class EsqueciSenhaViewModel
    {
        [Display(Name ="E-mail")]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
