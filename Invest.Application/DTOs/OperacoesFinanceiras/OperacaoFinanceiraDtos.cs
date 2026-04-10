using System.ComponentModel.DataAnnotations;
using Invest.Domain.Entities;
using Invest.Domain.Enums;

namespace Invest.Application.DTOs.OperacoesFinanceiras;

public sealed class OperacaoFinanceiraCreateDto
{
    [Required]
    public Guid CarteiraId { get; set; }

    [Required]
    public Guid AtivoId { get; set; }

    [Required]
    public TipoOperacao Tipo { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantidade { get; set; }

    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal PrecoUnitario { get; set; }

    [Required]
    public DateTime DataOperacao { get; set; }
}

public sealed class OperacaoFinanceiraReadDto
{
    public Guid Id { get; init; }
    public Guid CarteiraId { get; init; }
    public Guid AtivoId { get; init; }
    public TipoOperacao Tipo { get; init; }
    public int Quantidade { get; init; }
    public decimal PrecoUnitario { get; init; }
    public DateTime DataOperacao { get; init; }
    public DateTime DataCriacao { get; init; }

    public static OperacaoFinanceiraReadDto FromEntity(OperacaoFinanceira operacao) => new()
    {
        Id = operacao.Id,
        CarteiraId = operacao.CarteiraId,
        AtivoId = operacao.AtivoId,
        Tipo = operacao.Tipo,
        Quantidade = operacao.Quantidade,
        PrecoUnitario = operacao.PrecoUnitario,
        DataOperacao = operacao.DataOperacao,
        DataCriacao = operacao.DataCriacao
    };
}
