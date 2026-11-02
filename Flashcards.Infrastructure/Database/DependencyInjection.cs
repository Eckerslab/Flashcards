using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Flashcards.Infrastructure.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<FlashcardsDbContext>(o => o.UseSqlServer(connectionString));
        return services;
    }
}