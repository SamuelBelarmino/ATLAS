using Invest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Invest.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Carteira> Carteiras => Set<Carteira>();
    public DbSet<Ativo> Ativos => Set<Ativo>();
    public DbSet<OperacaoFinanceira> OperacoesFinanceiras => Set<OperacaoFinanceira>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
