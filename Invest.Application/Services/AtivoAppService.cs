using Invest.Application.Common.Exceptions;
using Invest.Application.Contracts;
using Invest.Application.Contracts.Persistence;
using Invest.Application.DTOs.Ativos;
using Invest.Domain.Entities;

namespace Invest.Application.Services;

public sealed class AtivoAppService : IAtivoAppService
{
    private readonly IAtivoRepository _ativoRepository;
    private readonly IOperacaoFinanceiraRepository _operacaoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AtivoAppService(
        IAtivoRepository ativoRepository,
        IOperacaoFinanceiraRepository operacaoRepository,
        IUnitOfWork unitOfWork)
    {
        _ativoRepository = ativoRepository;
        _operacaoRepository = operacaoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AtivoReadDto> CriarAsync(AtivoCreateDto dto, CancellationToken cancellationToken = default)
    {
        var codigo = dto.Codigo.Trim().ToUpperInvariant();
        if (await _ativoRepository.GetByCodigoAsync(codigo, cancellationToken) is not null)
        {
            throw new BusinessRuleException("Já existe um ativo cadastrado com este código.");
        }

        var ativo = new Ativo
        {
            Id = Guid.NewGuid(),
            Codigo = codigo,
            Tipo = dto.Tipo,
            Mercado = dto.Mercado
        };

        await _ativoRepository.AddAsync(ativo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return AtivoReadDto.FromEntity(ativo);
    }

    public async Task<AtivoReadDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var ativo = await _ativoRepository.GetByIdAsync(id, cancellationToken);
        return ativo is null ? null : AtivoReadDto.FromEntity(ativo);
    }

    public async Task<IReadOnlyList<AtivoReadDto>> ListarAsync(CancellationToken cancellationToken = default)
    {
        var ativos = await _ativoRepository.GetAllAsync(cancellationToken);
        return ativos.Select(AtivoReadDto.FromEntity).ToList();
    }

    public async Task<AtivoReadDto> AtualizarAsync(Guid id, AtivoUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var ativo = await _ativoRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Ativo não encontrado.");

        var codigo = dto.Codigo.Trim().ToUpperInvariant();
        var existente = await _ativoRepository.GetByCodigoAsync(codigo, cancellationToken);
        if (existente is not null && existente.Id != id)
        {
            throw new BusinessRuleException("Já existe um ativo cadastrado com este código.");
        }

        ativo.Codigo = codigo;
        ativo.Tipo = dto.Tipo;
        ativo.Mercado = dto.Mercado;

        _ativoRepository.Update(ativo);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return AtivoReadDto.FromEntity(ativo);
    }

    public async Task RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var ativo = await _ativoRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Ativo não encontrado.");

        var operacoes = await _operacaoRepository.GetByAtivoIdAsync(id, cancellationToken);
        if (operacoes.Count > 0)
        {
            throw new BusinessRuleException("Não é possível remover o ativo porque existem operações vinculadas.");
        }

        _ativoRepository.Remove(ativo);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
