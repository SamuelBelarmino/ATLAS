using Invest.Domain.Enums;

namespace Invest.Domain.Entities;

public class OperacaoFinanceira
{
    public Guid Id { get; set; }
    public Guid CarteiraId { get; set; }
    public Guid AtivoId { get; set; }
    public TipoOperacao Tipo { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public DateTime DataOperacao { get; set; }
    public DateTime DataCriacao { get; set; }

    // Navigation Properties
    public Carteira Carteira { get; set; } = null!;
    public Ativo Ativo { get; set; } = null!;
}
