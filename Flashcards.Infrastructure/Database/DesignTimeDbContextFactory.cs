using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Flashcards.Infrastructure.Database;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FlashcardsDbContext>
{
    public FlashcardsDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<FlashcardsDbContext> optionsBuilder = new();
        var connectionString = ReadAppSettings("ConnectionStrings", "DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        return new FlashcardsDbContext(optionsBuilder.Options);

        static string? ReadAppSettings(string property, string key)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(
                "StudentAdministrationDatabase.appsettings.json");

            if (stream is null) return null;

            using var doc = JsonDocument.Parse(stream);
            return doc.RootElement.GetProperty(property).GetProperty(key).GetString();
        }
    }
}