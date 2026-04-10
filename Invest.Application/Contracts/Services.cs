using Invest.Application.Common.Models;
using Invest.Application.DTOs.Ativos;
using Invest.Application.DTOs.Carteiras;
using Invest.Application.DTOs.OperacoesFinanceiras;
using Invest.Application.DTOs.Usuarios;

namespace Invest.Application.Contracts;

public interface IUsuarioAppService
{
    Task<UsuarioReadDto> CriarAsync(UsuarioCreateDto dto, CancellationToken cancellationToken = default);
    Task<UsuarioReadDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<UsuarioReadDto>> ListarAsync(CancellationToken cancellationToken = default);
    Task<UsuarioReadDto> AtualizarAsync(Guid id, UsuarioUpdateDto dto, CancellationToken cancellationToken = default);
    Task RemoverAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface ICarteiraAppService
{
    Task<CarteiraReadDto> CriarAsync(CarteiraCreateDto dto, CancellationToken cancellationToken = default);
    Task<CarteiraReadDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<CarteiraReadDto>> ListarAsync(CarteiraQueryDto query, CancellationToken cancellationToken = default);
    Task<CarteiraReadDto> AtualizarAsync(Guid id, CarteiraUpdateDto dto, CancellationToken cancellationToken = default);
    Task RemoverAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface IAtivoAppService
{
    Task<AtivoReadDto> CriarAsync(AtivoCreateDto dto, CancellationToken cancellationToken = default);
    Task<AtivoReadDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AtivoReadDto>> ListarAsync(CancellationToken cancellationToken = default);
    Task<AtivoReadDto> AtualizarAsync(Guid id, AtivoUpdateDto dto, CancellationToken cancellationToken = default);
    Task RemoverAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface IOperacaoFinanceiraAppService
{
    Task<OperacaoFinanceiraReadDto> CriarAsync(OperacaoFinanceiraCreateDto dto, CancellationToken cancellationToken = default);
    Task<OperacaoFinanceiraReadDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<OperacaoFinanceiraReadDto>> ListarAsync(OperacaoFinanceiraQueryDto query, CancellationToken cancellationToken = default);
}
