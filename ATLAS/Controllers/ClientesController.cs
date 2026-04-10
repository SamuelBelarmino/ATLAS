using Invest.Application.Contracts;
using Invest.Application.DTOs.Usuarios;
using Microsoft.AspNetCore.Mvc;

namespace Invest.API.Controllers;

[ApiController]
[Route("api/clientes")]
public sealed class ClientesController : ControllerBase
{
    private readonly IUsuarioAppService _usuarioAppService;

    public ClientesController(IUsuarioAppService usuarioAppService)
    {
        _usuarioAppService = usuarioAppService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UsuarioReadDto>>> Listar(CancellationToken cancellationToken)
    {
        var usuarios = await _usuarioAppService.ListarAsync(cancellationToken);
        return Ok(usuarios);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UsuarioReadDto>> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioAppService.ObterPorIdAsync(id, cancellationToken);
        return usuario is null ? NotFound() : Ok(usuario);
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioReadDto>> Criar([FromBody] UsuarioCreateDto dto, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioAppService.CriarAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(ObterPorId), new { id = usuario.Id }, usuario);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UsuarioReadDto>> Atualizar(Guid id, [FromBody] UsuarioUpdateDto dto, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioAppService.AtualizarAsync(id, dto, cancellationToken);
        return Ok(usuario);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remover(Guid id, CancellationToken cancellationToken)
    {
        await _usuarioAppService.RemoverAsync(id, cancellationToken);
        return NoContent();
    }
}
