using Invest.Application.Contracts;
using Invest.Application.DTOs.Ativos;
using Microsoft.AspNetCore.Mvc;

namespace Invest.API.Controllers;

[ApiController]
[Route("api/ativos")]
public sealed class AtivosController : ControllerBase
{
    private readonly IAtivoAppService _ativoAppService;

    public AtivosController(IAtivoAppService ativoAppService)
    {
        _ativoAppService = ativoAppService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AtivoReadDto>>> Listar(CancellationToken cancellationToken)
    {
        var ativos = await _ativoAppService.ListarAsync(cancellationToken);
        return Ok(ativos);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AtivoReadDto>> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var ativo = await _ativoAppService.ObterPorIdAsync(id, cancellationToken);
        return ativo is null ? NotFound() : Ok(ativo);
    }

    [HttpPost]
    public async Task<ActionResult<AtivoReadDto>> Criar([FromBody] AtivoCreateDto dto, CancellationToken cancellationToken)
    {
        var ativo = await _ativoAppService.CriarAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(ObterPorId), new { id = ativo.Id }, ativo);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AtivoReadDto>> Atualizar(Guid id, [FromBody] AtivoUpdateDto dto, CancellationToken cancellationToken)
    {
        var ativo = await _ativoAppService.AtualizarAsync(id, dto, cancellationToken);
        return Ok(ativo);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remover(Guid id, CancellationToken cancellationToken)
    {
        await _ativoAppService.RemoverAsync(id, cancellationToken);
        return NoContent();
    }
}
