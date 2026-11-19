using Flashcards.Application.Abstractions;
using Flashcards.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Flashcards.Infrastructure.Database;

/// <summary>
///     Provides extension methods to register the infrastructure services with the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the database context and the repositories as scoped services.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="connectionString">The connection string of the SQL Server database.</param>
    /// <returns>The same service collection, so that further calls can be chained.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<FlashcardsDbContext>(o => o.UseSqlServer(connectionString));

        services.AddScoped<IBoxRepository, BoxRepository>();
        services.AddScoped<ICardRepository, CardRepository>();

        return services;
    }
}
