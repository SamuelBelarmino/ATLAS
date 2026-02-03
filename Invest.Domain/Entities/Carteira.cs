using Invest.Domain.Enums;

namespace Invest.Domain.Entities;

public class Carteira
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public PerfilRisco PerfilRisco { get; set; }
    public DateTime DataCriacao { get; set; }

    // Navigation Properties
    public Usuario Usuario { get; set; } = null!;
    public ICollection<OperacaoFinanceira> Operacoes { get; set; } = [];
}
