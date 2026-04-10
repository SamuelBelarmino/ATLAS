using Invest.Application.Common.Models;
using Invest.Application.DTOs.Carteiras;
using Invest.Application.DTOs.OperacoesFinanceiras;
using Invest.Domain.Entities;

namespace Invest.Application.Contracts.Persistence;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Usuario>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Usuario usuario, CancellationToken cancellationToken = default);
    void Update(Usuario usuario);
    void Remove(Usuario usuario);
}

public interface ICarteiraRepository
{
    Task<Carteira?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<Carteira>> SearchAsync(CarteiraQueryDto query, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Carteira>> GetByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task AddAsync(Carteira carteira, CancellationToken cancellationToken = default);
    void Update(Carteira carteira);
    void Remove(Carteira carteira);
}

public interface IAtivoRepository
{
    Task<Ativo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Ativo?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Ativo>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Ativo ativo, CancellationToken cancellationToken = default);
    void Update(Ativo ativo);
    void Remove(Ativo ativo);
}

public interface IOperacaoFinanceiraRepository
{
    Task<OperacaoFinanceira?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<OperacaoFinanceira>> SearchAsync(OperacaoFinanceiraQueryDto query, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OperacaoFinanceira>> GetByCarteiraIdAsync(Guid carteiraId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OperacaoFinanceira>> GetByAtivoIdAsync(Guid ativoId, CancellationToken cancellationToken = default);
    Task AddAsync(OperacaoFinanceira operacao, CancellationToken cancellationToken = default);
    void Remove(OperacaoFinanceira operacao);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
