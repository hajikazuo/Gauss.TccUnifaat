using System.ComponentModel.DataAnnotations;

namespace Gauss.TccUnifaat.MVC.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um endereço de email válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$",
        ErrorMessage = "A senha deve conter pelo menos 8 caracteres, uma letra maiúscula, um número e um caractere especial.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O campo Confirme a senha é obrigatório.")]
        [Display(Name = "Confirme a senha")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "As senhas não conferem")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "O Nome é obrigatório.")]
        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O Cpf é obrigatório.")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Display(Name = "Número do telefone")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O campo Idade é obrigatório.")]
        [Display(Name = "Idade")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "Selecione uma função.")]
        [Display(Name = "Função")]
        public string SelectedRole { get; set; }
    }
}
