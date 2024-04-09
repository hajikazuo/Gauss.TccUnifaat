using Gauss.TccUnifaat.Common.Models;

namespace Gauss.TccUnifaat.MVC.Dapper
{
    public class UsuariosViewModel
    {
        public Guid Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string Turma { get; set; }
        public string Funcao { get; set; }
        public int QuantidadeUsuariosPorTurma { get; set; }
    }
}
