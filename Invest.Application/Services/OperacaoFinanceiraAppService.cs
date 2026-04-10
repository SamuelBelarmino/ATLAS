using Invest.Application.Common.Exceptions;
using Invest.Application.Common.Models;
using Invest.Application.Contracts;
using Invest.Application.Contracts.Persistence;
using Invest.Application.DTOs.OperacoesFinanceiras;
using Invest.Domain.Entities;

namespace Invest.Application.Services;

public sealed class OperacaoFinanceiraAppService : IOperacaoFinanceiraAppService
{
    private readonly IOperacaoFinanceiraRepository _operacaoRepository;
    private readonly ICarteiraRepository _carteiraRepository;
    private readonly IAtivoRepository _ativoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OperacaoFinanceiraAppService(
        IOperacaoFinanceiraRepository operacaoRepository,
        ICarteiraRepository carteiraRepository,
        IAtivoRepository ativoRepository,
        IUnitOfWork unitOfWork)
    {
        _operacaoRepository = operacaoRepository;
        _carteiraRepository = carteiraRepository;
        _ativoRepository = ativoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OperacaoFinanceiraReadDto> CriarAsync(OperacaoFinanceiraCreateDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.Quantidade <= 0)
        {
            throw new BusinessRuleException("A quantidade deve ser maior que zero.");
        }

        if (dto.PrecoUnitario <= 0)
        {
            throw new BusinessRuleException("O preço unitário deve ser maior que zero.");
        }

        if (await _carteiraRepository.GetByIdAsync(dto.CarteiraId, cancellationToken) is null)
        {
            throw new BusinessRuleException("Carteira informada não existe.");
        }

        if (await _ativoRepository.GetByIdAsync(dto.AtivoId, cancellationToken) is null)
        {
            throw new BusinessRuleException("Ativo informado não existe.");
        }

        var operacao = new OperacaoFinanceira
        {
            Id = Guid.NewGuid(),
            CarteiraId = dto.CarteiraId,
            AtivoId = dto.AtivoId,
            Tipo = dto.Tipo,
            Quantidade = dto.Quantidade,
            PrecoUnitario = dto.PrecoUnitario,
            DataOperacao = dto.DataOperacao,
            DataCriacao = DateTime.UtcNow
        };

        await _operacaoRepository.AddAsync(operacao, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return OperacaoFinanceiraReadDto.FromEntity(operacao);
    }

    public async Task<OperacaoFinanceiraReadDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var operacao = await _operacaoRepository.GetByIdAsync(id, cancellationToken);
        return operacao is null ? null : OperacaoFinanceiraReadDto.FromEntity(operacao);
    }

    public async Task<PagedResult<OperacaoFinanceiraReadDto>> ListarAsync(OperacaoFinanceiraQueryDto query, CancellationToken cancellationToken = default)
    {
        var result = await _operacaoRepository.SearchAsync(query, cancellationToken);
        return new PagedResult<OperacaoFinanceiraReadDto>
        {
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            Items = result.Items.Select(OperacaoFinanceiraReadDto.FromEntity).ToList()
        };
    }
}
