using Invest.Application.Contracts;
using Invest.Application.DTOs.Carteiras;
using Microsoft.AspNetCore.Mvc;

namespace Invest.API.Controllers;

[ApiController]
[Route("api/carteiras")]
public sealed class CarteirasController : ControllerBase
{
    private readonly ICarteiraAppService _carteiraAppService;

    public CarteirasController(ICarteiraAppService carteiraAppService)
    {
        _carteiraAppService = carteiraAppService;
    }

    [HttpGet]
    public async Task<ActionResult> Listar([FromQuery] CarteiraQueryDto query, CancellationToken cancellationToken)
    {
        var carteiras = await _carteiraAppService.ListarAsync(query, cancellationToken);
        return Ok(carteiras);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CarteiraReadDto>> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var carteira = await _carteiraAppService.ObterPorIdAsync(id, cancellationToken);
        return carteira is null ? NotFound() : Ok(carteira);
    }

    [HttpPost]
    public async Task<ActionResult<CarteiraReadDto>> Criar([FromBody] CarteiraCreateDto dto, CancellationToken cancellationToken)
    {
        var carteira = await _carteiraAppService.CriarAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(ObterPorId), new { id = carteira.Id }, carteira);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CarteiraReadDto>> Atualizar(Guid id, [FromBody] CarteiraUpdateDto dto, CancellationToken cancellationToken)
    {
        var carteira = await _carteiraAppService.AtualizarAsync(id, dto, cancellationToken);
        return Ok(carteira);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remover(Guid id, CancellationToken cancellationToken)
    {
        await _carteiraAppService.RemoverAsync(id, cancellationToken);
        return NoContent();
    }
}
