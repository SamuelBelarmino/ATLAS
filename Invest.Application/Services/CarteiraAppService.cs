using Invest.Application.Common.Exceptions;
using Invest.Application.Common.Models;
using Invest.Application.Contracts;
using Invest.Application.Contracts.Persistence;
using Invest.Application.DTOs.Carteiras;
using Invest.Domain.Entities;

namespace Invest.Application.Services;

public sealed class CarteiraAppService : ICarteiraAppService
{
    private readonly ICarteiraRepository _carteiraRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IOperacaoFinanceiraRepository _operacaoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CarteiraAppService(
        ICarteiraRepository carteiraRepository,
        IUsuarioRepository usuarioRepository,
        IOperacaoFinanceiraRepository operacaoRepository,
        IUnitOfWork unitOfWork)
    {
        _carteiraRepository = carteiraRepository;
        _usuarioRepository = usuarioRepository;
        _operacaoRepository = operacaoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CarteiraReadDto> CriarAsync(CarteiraCreateDto dto, CancellationToken cancellationToken = default)
    {
        if (await _usuarioRepository.GetByIdAsync(dto.UsuarioId, cancellationToken) is null)
        {
            throw new BusinessRuleException("Cliente informado não existe.");
        }

        var carteira = new Carteira
        {
            Id = Guid.NewGuid(),
            UsuarioId = dto.UsuarioId,
            Nome = dto.Nome.Trim(),
            PerfilRisco = dto.PerfilRisco,
            DataCriacao = DateTime.UtcNow
        };

        await _carteiraRepository.AddAsync(carteira, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return CarteiraReadDto.FromEntity(carteira);
    }

    public async Task<CarteiraReadDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var carteira = await _carteiraRepository.GetByIdAsync(id, cancellationToken);
        return carteira is null ? null : CarteiraReadDto.FromEntity(carteira);
    }

    public async Task<PagedResult<CarteiraReadDto>> ListarAsync(CarteiraQueryDto query, CancellationToken cancellationToken = default)
    {
        var result = await _carteiraRepository.SearchAsync(query, cancellationToken);
        return new PagedResult<CarteiraReadDto>
        {
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            Items = result.Items.Select(CarteiraReadDto.FromEntity).ToList()
        };
    }

    public async Task<CarteiraReadDto> AtualizarAsync(Guid id, CarteiraUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var carteira = await _carteiraRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Carteira não encontrada.");

        if (await _usuarioRepository.GetByIdAsync(dto.UsuarioId, cancellationToken) is null)
        {
            throw new BusinessRuleException("Cliente informado não existe.");
        }

        carteira.UsuarioId = dto.UsuarioId;
        carteira.Nome = dto.Nome.Trim();
        carteira.PerfilRisco = dto.PerfilRisco;

        _carteiraRepository.Update(carteira);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return CarteiraReadDto.FromEntity(carteira);
    }

    public async Task RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var carteira = await _carteiraRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Carteira não encontrada.");

        var operacoes = await _operacaoRepository.GetByCarteiraIdAsync(id, cancellationToken);
        if (operacoes.Count > 0)
        {
            throw new BusinessRuleException("Não é possível remover a carteira porque existem operações vinculadas.");
        }

        _carteiraRepository.Remove(carteira);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
