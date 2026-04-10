using Invest.Application.Common.Models;
using Invest.Application.Contracts.Persistence;
using Invest.Application.DTOs.Carteiras;
using Invest.Domain.Entities;
using Invest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Invest.Infrastructure.Persistence.Repositories;

public sealed class CarteiraRepository : ICarteiraRepository
{
    private readonly AppDbContext _context;

    public CarteiraRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Carteira?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Carteiras.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<PagedResult<Carteira>> SearchAsync(CarteiraQueryDto query, CancellationToken cancellationToken = default)
    {
        var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
        var pageSize = query.PageSize < 1 ? 10 : Math.Min(query.PageSize, 100);

        var baseQuery = _context.Carteiras.AsNoTracking().AsQueryable();

        if (query.UsuarioId.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.UsuarioId == query.UsuarioId.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.Nome))
        {
            var nome = query.Nome.Trim();
            baseQuery = baseQuery.Where(x => x.Nome.Contains(nome));
        }

        if (query.PerfilRisco.HasValue)
        {
            baseQuery = baseQuery.Where(x => x.PerfilRisco == query.PerfilRisco.Value);
        }

        var totalCount = await baseQuery.CountAsync(cancellationToken);
        var items = await baseQuery
            .OrderBy(x => x.Nome)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Carteira>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items
        };
    }

    public async Task<IReadOnlyList<Carteira>> GetByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default) =>
        await _context.Carteiras.AsNoTracking().Where(x => x.UsuarioId == usuarioId).OrderBy(x => x.Nome).ToListAsync(cancellationToken);

    public Task AddAsync(Carteira carteira, CancellationToken cancellationToken = default) =>
        _context.Carteiras.AddAsync(carteira, cancellationToken).AsTask();

    public void Update(Carteira carteira) => _context.Carteiras.Update(carteira);

    public void Remove(Carteira carteira) => _context.Carteiras.Remove(carteira);
}
