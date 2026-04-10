using Invest.Application.Contracts.Persistence;
using Invest.Infrastructure.Data;
using Invest.Infrastructure.Persistence;
using Invest.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Invest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ICarteiraRepository, CarteiraRepository>();
        services.AddScoped<IAtivoRepository, AtivoRepository>();
        services.AddScoped<IOperacaoFinanceiraRepository, OperacaoFinanceiraRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
