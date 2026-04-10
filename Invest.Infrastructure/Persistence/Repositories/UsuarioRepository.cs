using Invest.Application.Contracts.Persistence;
using Invest.Domain.Entities;
using Invest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Invest.Infrastructure.Persistence.Repositories;

public sealed class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        _context.Usuarios.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public async Task<IReadOnlyList<Usuario>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.Usuarios.AsNoTracking().OrderBy(x => x.Nome).ToListAsync(cancellationToken);

    public Task AddAsync(Usuario usuario, CancellationToken cancellationToken = default) =>
        _context.Usuarios.AddAsync(usuario, cancellationToken).AsTask();

    public void Update(Usuario usuario) => _context.Usuarios.Update(usuario);

    public void Remove(Usuario usuario) => _context.Usuarios.Remove(usuario);
}
