using Invest.Domain.Enums;

namespace Invest.Application.DTOs.Carteiras;

public sealed class CarteiraQueryDto
{
    public Guid? UsuarioId { get; set; }
    public string? Nome { get; set; }
    public PerfilRisco? PerfilRisco { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
