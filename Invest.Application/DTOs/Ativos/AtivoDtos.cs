using System.ComponentModel.DataAnnotations;
using Invest.Domain.Entities;
using Invest.Domain.Enums;

namespace Invest.Application.DTOs.Ativos;

public sealed class AtivoCreateDto
{
    [Required]
    [MaxLength(20)]
    public string Codigo { get; set; } = string.Empty;

    [Required]
    public TipoAtivo Tipo { get; set; }

    [Required]
    public Mercado Mercado { get; set; }
}

public sealed class AtivoUpdateDto
{
    [Required]
    [MaxLength(20)]
    public string Codigo { get; set; } = string.Empty;

    [Required]
    public TipoAtivo Tipo { get; set; }

    [Required]
    public Mercado Mercado { get; set; }
}

public sealed class AtivoReadDto
{
    public Guid Id { get; init; }
    public string Codigo { get; init; } = string.Empty;
    public TipoAtivo Tipo { get; init; }
    public Mercado Mercado { get; init; }

    public static AtivoReadDto FromEntity(Ativo ativo) => new()
    {
        Id = ativo.Id,
        Codigo = ativo.Codigo,
        Tipo = ativo.Tipo,
        Mercado = ativo.Mercado
    };
}
