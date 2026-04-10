using System.ComponentModel.DataAnnotations;
using Invest.Domain.Entities;
using Invest.Domain.Enums;

namespace Invest.Application.DTOs.Carteiras;

public sealed class CarteiraCreateDto
{
    [Required]
    public Guid UsuarioId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public PerfilRisco PerfilRisco { get; set; }
}

public sealed class CarteiraUpdateDto
{
    [Required]
    public Guid UsuarioId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public PerfilRisco PerfilRisco { get; set; }
}

public sealed class CarteiraReadDto
{
    public Guid Id { get; init; }
    public Guid UsuarioId { get; init; }
    public string Nome { get; init; } = string.Empty;
    public PerfilRisco PerfilRisco { get; init; }
    public DateTime DataCriacao { get; init; }

    public static CarteiraReadDto FromEntity(Carteira carteira) => new()
    {
        Id = carteira.Id,
        UsuarioId = carteira.UsuarioId,
        Nome = carteira.Nome,
        PerfilRisco = carteira.PerfilRisco,
        DataCriacao = carteira.DataCriacao
    };
}
