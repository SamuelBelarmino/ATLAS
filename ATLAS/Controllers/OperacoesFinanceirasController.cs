using Invest.Application.Contracts;
using Invest.Application.DTOs.OperacoesFinanceiras;
using Microsoft.AspNetCore.Mvc;

namespace Invest.API.Controllers;

[ApiController]
[Route("api/operacoes")]
public sealed class OperacoesFinanceirasController : ControllerBase
{
    private readonly IOperacaoFinanceiraAppService _operacaoAppService;

    public OperacoesFinanceirasController(IOperacaoFinanceiraAppService operacaoAppService)
    {
        _operacaoAppService = operacaoAppService;
    }

    [HttpGet]
    public async Task<ActionResult> Listar([FromQuery] OperacaoFinanceiraQueryDto query, CancellationToken cancellationToken)
    {
        var operacoes = await _operacaoAppService.ListarAsync(query, cancellationToken);
        return Ok(operacoes);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OperacaoFinanceiraReadDto>> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var operacao = await _operacaoAppService.ObterPorIdAsync(id, cancellationToken);
        return operacao is null ? NotFound() : Ok(operacao);
    }

    [HttpPost]
    public async Task<ActionResult<OperacaoFinanceiraReadDto>> Criar([FromBody] OperacaoFinanceiraCreateDto dto, CancellationToken cancellationToken)
    {
        var operacao = await _operacaoAppService.CriarAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(ObterPorId), new { id = operacao.Id }, operacao);
    }
}
