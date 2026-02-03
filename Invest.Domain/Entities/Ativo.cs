using Invest.Domain.Enums;

namespace Invest.Domain.Entities;

public class Ativo
{
    public Guid Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public TipoAtivo Tipo { get; set; }
    public Mercado Mercado { get; set; }

    // Navigation Properties
    public ICollection<OperacaoFinanceira> Operacoes { get; set; } = [];
}
