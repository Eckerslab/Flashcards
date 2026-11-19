using Flashcards.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Infrastructure.Database;

/// <summary>
///     Represents the Entity Framework Core database context of the Flashcards application.
/// </summary>
/// <param name="options">The options used to configure the context.</param>
public class FlashcardsDbContext(DbContextOptions<FlashcardsDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets the set of boxes stored in the database.
    /// </summary>
    public DbSet<BoxModel> Boxes => Set<BoxModel>();

    /// <summary>
    /// Gets the set of cards stored in the database.
    /// </summary>
    public DbSet<CardModel> Cards => Set<CardModel>();

    /// <summary>
    /// Configures the database schema, including column lengths, indexes and the relationship between boxes and cards.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BoxModel>(b =>
        {
            b.Property(x => x.Name).HasMaxLength(100).IsRequired();
            b.HasIndex(x => x.UserId);
        });
        modelBuilder.Entity<CardModel>(f =>
        {
            f.Property(x => x.Question).HasMaxLength(500).IsRequired();
            f.Property(x => x.Answer).HasMaxLength(500).IsRequired();
            f.HasIndex(x => new { x.BoxId, x.DueDate });
            f.HasOne<BoxModel>().WithMany().HasForeignKey(x => x.BoxId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
