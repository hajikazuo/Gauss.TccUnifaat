namespace Gauss.TccUnifaat.MVC.Dapper
{
    public class ControleFaltasViewModel
    {
        public Guid UsuarioId { get; set; }
        public string NomeCompleto { get; set; }
        public Guid TurmaId { get; set; }
        public string NomeTurma { get; set; }
        public int QtdeFaltas { get; set; }
    }
}
