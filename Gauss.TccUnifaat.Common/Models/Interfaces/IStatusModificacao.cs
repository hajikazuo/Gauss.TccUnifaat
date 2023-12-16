namespace Gauss.TccUnifaat.Common.Models
{
    public interface IStatusModificacao
    {
        bool Excluido { get; set; }

        DateTime? DataExcluido { get; set; }

        DateTime DataCadastro { get; set; }

        DateTime? DataUltimaModificacao { get; set; }
    }
}
