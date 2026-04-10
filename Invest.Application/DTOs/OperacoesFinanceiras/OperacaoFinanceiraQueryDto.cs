using Invest.Domain.Enums;

namespace Invest.Application.DTOs.OperacoesFinanceiras;

public sealed class OperacaoFinanceiraQueryDto
{
    public Guid? CarteiraId { get; set; }
    public Guid? AtivoId { get; set; }
    public TipoOperacao? Tipo { get; set; }
    public DateTime? DataInicial { get; set; }
    public DateTime? DataFinal { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
