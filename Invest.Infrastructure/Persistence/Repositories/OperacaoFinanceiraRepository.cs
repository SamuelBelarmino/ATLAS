using Invest.Application.Common.Models;
using Invest.Application.Contracts.Persistence;
using Invest.Application.DTOs.OperacoesFinanceiras;
using Invest.Domain.Entities;
using Invest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Invest.Infrastructure.Persistence.Repositories;

public sealed class OperacaoFinanceiraRepository : IOperacaoFinanceiraRepository
{
    private readonly AppDbContext _context;

    public OperacaoFinanceiraRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<OperacaoFinanceira?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.OperacoesFinanceiras.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<PagedResult<OperacaoFinanceira>> SearchAsync(OperacaoFinanceiraQueryDto query, CancellationToken cancellationToken = default)
    {
        var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
        var pageSize = query.PageSize < 1 ? 10 : Math.Min(query.PageSize, 100);

        var baseQuery = _context.OperacoesFinanceiras.AsNoTracking().AsQueryable();

        if (query.CarteiraId.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.CarteiraId == query.CarteiraId.Value);
        }

        if (query.AtivoId.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.AtivoId == query.AtivoId.Value);
        }

        if (query.Tipo.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.Tipo == query.Tipo.Value);
        }

        if (query.DataInicial.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.DataOperacao >= query.DataInicial.Value);
        }

        if (query.DataFinal.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.DataOperacao <= query.DataFinal.Value);
        }

        var totalCount = await baseQuery.CountAsync(cancellationToken);
        var items = await baseQuery
            .OrderByDescending(x => x.DataOperacao)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<OperacaoFinanceira>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items
        };
    }

    public async Task<IReadOnlyList<OperacaoFinanceira>> GetByCarteiraIdAsync(Guid carteiraId, CancellationToken cancellationToken = default) =>
        await _context.OperacoesFinanceiras.AsNoTracking().Where(x => x.CarteiraId == carteiraId).OrderByDescending(x => x.DataOperacao).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<OperacaoFinanceira>> GetByAtivoIdAsync(Guid ativoId, CancellationToken cancellationToken = default) =>
        await _context.OperacoesFinanceiras.AsNoTracking().Where(x => x.AtivoId == ativoId).OrderByDescending(x => x.DataOperacao).ToListAsync(cancellationToken);

    public Task AddAsync(OperacaoFinanceira operacao, CancellationToken cancellationToken = default) =>
        _context.OperacoesFinanceiras.AddAsync(operacao, cancellationToken).AsTask();

    public void Remove(OperacaoFinanceira operacao) => _context.OperacoesFinanceiras.Remove(operacao);
}
