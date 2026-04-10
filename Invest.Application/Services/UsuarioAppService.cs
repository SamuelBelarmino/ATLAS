using Invest.Application.Common.Exceptions;
using Invest.Application.Contracts;
using Invest.Application.Contracts.Persistence;
using Invest.Application.DTOs.Usuarios;
using Invest.Domain.Entities;

namespace Invest.Application.Services;

public sealed class UsuarioAppService : IUsuarioAppService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ICarteiraRepository _carteiraRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UsuarioAppService(
        IUsuarioRepository usuarioRepository,
        ICarteiraRepository carteiraRepository,
        IUnitOfWork unitOfWork)
    {
        _usuarioRepository = usuarioRepository;
        _carteiraRepository = carteiraRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UsuarioReadDto> CriarAsync(UsuarioCreateDto dto, CancellationToken cancellationToken = default)
    {
        var email = dto.Email.Trim().ToLowerInvariant();
        if (await _usuarioRepository.GetByEmailAsync(email, cancellationToken) is not null)
        {
            throw new BusinessRuleException("Já existe um cliente cadastrado com este e-mail.");
        }

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome.Trim(),
            Email = email,
            SenhaHash = dto.SenhaHash,
            DataCriacao = DateTime.UtcNow
        };

        await _usuarioRepository.AddAsync(usuario, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return UsuarioReadDto.FromEntity(usuario);
    }

    public async Task<UsuarioReadDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id, cancellationToken);
        return usuario is null ? null : UsuarioReadDto.FromEntity(usuario);
    }

    public async Task<IReadOnlyList<UsuarioReadDto>> ListarAsync(CancellationToken cancellationToken = default)
    {
        var usuarios = await _usuarioRepository.GetAllAsync(cancellationToken);
        return usuarios.Select(UsuarioReadDto.FromEntity).ToList();
    }

    public async Task<UsuarioReadDto> AtualizarAsync(Guid id, UsuarioUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Cliente não encontrado.");

        var email = dto.Email.Trim().ToLowerInvariant();
        var existente = await _usuarioRepository.GetByEmailAsync(email, cancellationToken);
        if (existente is not null && existente.Id != id)
        {
            throw new BusinessRuleException("Já existe um cliente cadastrado com este e-mail.");
        }

        usuario.Nome = dto.Nome.Trim();
        usuario.Email = email;
        usuario.SenhaHash = dto.SenhaHash;

        _usuarioRepository.Update(usuario);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return UsuarioReadDto.FromEntity(usuario);
    }

    public async Task RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Cliente não encontrado.");

        var carteiras = await _carteiraRepository.GetByUsuarioIdAsync(id, cancellationToken);
        if (carteiras.Count > 0)
        {
            throw new BusinessRuleException("Não é possível remover o cliente porque ele possui carteiras vinculadas.");
        }

        _usuarioRepository.Remove(usuario);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
