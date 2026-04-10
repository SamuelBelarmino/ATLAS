using Invest.Application.Contracts.Persistence;
using Invest.Domain.Entities;
using Invest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Invest.Infrastructure.Persistence.Repositories;

public sealed class AtivoRepository : IAtivoRepository
{
    private readonly AppDbContext _context;

    public AtivoRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Ativo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Ativos.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Ativo?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default) =>
        _context.Ativos.FirstOrDefaultAsync(x => x.Codigo == codigo, cancellationToken);

    public async Task<IReadOnlyList<Ativo>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.Ativos.AsNoTracking().OrderBy(x => x.Codigo).ToListAsync(cancellationToken);

    public Task AddAsync(Ativo ativo, CancellationToken cancellationToken = default) =>
        _context.Ativos.AddAsync(ativo, cancellationToken).AsTask();

    public void Update(Ativo ativo) => _context.Ativos.Update(ativo);

    public void Remove(Ativo ativo) => _context.Ativos.Remove(ativo);
}
