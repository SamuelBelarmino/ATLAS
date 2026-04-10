using Invest.Application.Contracts;
using Invest.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Invest.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioAppService, UsuarioAppService>();
        services.AddScoped<ICarteiraAppService, CarteiraAppService>();
        services.AddScoped<IAtivoAppService, AtivoAppService>();
        services.AddScoped<IOperacaoFinanceiraAppService, OperacaoFinanceiraAppService>();

        return services;
    }
}
